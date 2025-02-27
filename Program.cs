using System.Reflection;
using EShop.API.Data;
using EShop.API.Data.Repositories.Interfaces;
using EShop.API.Data.Repositories;
using EShop.API.Services.Interfaces;
using EShop.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.Filters;
using static EShop.API.Controllers.LiquidsController;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EShop.API.Models.Users;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация БД
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Конфигурация идентификации
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Конфигурация аутентификации
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Регистрация репозиториев
builder.Services.AddScoped<ILiquidRepository, LiquidRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();

// Регистрация сервисов
builder.Services.AddScoped<ILiquidService, LiquidService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IManufacturerService, ManufacturerService>();

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTelegram", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Конфигурация контроллеров
builder.Services.AddControllers();

// Настройка Swagger с комментариями
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EShop API",
        Version = "v1",
        Description = "API для Интернет-магазина"
    });

    c.EnableAnnotations();
    c.ExampleFilters();

    // Включение XML-комментариев
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<LiquidModelExample>();

var app = builder.Build();

// Автомиграции при запуске
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Инициализация ролей при запуске
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Создание ролей
    var roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Создание администратора
    var adminEmail = "admin@csharp.by";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User"
        };

        var result = await userManager.CreateAsync(adminUser, "5Gh2eub8133!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

app.UsePathBase("/api");
app.UseRouting();

// Swagger configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "EShop API V1");
        c.RoutePrefix = "swagger";

        // Сворачиваем все методы по умолчанию
        c.ConfigObject.DisplayRequestDuration = true;
        c.ConfigObject.DocExpansion = DocExpansion.None;
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "EShop API V1");
        c.RoutePrefix = "docs"; // измените путь
    });
    app.Use(async (context, next) =>
    {
        if (context.Request.Path.StartsWithSegments("/docs"))
        {
            // Добавьте аутентификацию
        }
        await next();
    });
}

// Middleware pipeline
app.UseHttpsRedirection();

app.UseCors("AllowTelegram");
app.UseAuthorization();
app.MapControllers();

app.Run();
