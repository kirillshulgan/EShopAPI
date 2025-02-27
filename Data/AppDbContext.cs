using EShop.API.Models;
using EShop.API.Models.Components;
using EShop.API.Models.Devices;
using EShop.API.Models.Liquids;
using EShop.API.Models.Manufacturers;
using EShop.API.Models.StockCounts;
using EShop.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShop.API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LiquidModel> Liquids => Set<LiquidModel>();
        public DbSet<DeviceModel> Devices => Set<DeviceModel>();
        public DbSet<ComponentModel> Components => Set<ComponentModel>();
        public DbSet<ManufacturerModel> Manufacturers => Set<ManufacturerModel>();
        public DbSet<LiquidStockCountModel> LiquidStockCounts => Set<LiquidStockCountModel>();
        public DbSet<DeviceStockCountModel> DeviceStockCounts => Set<DeviceStockCountModel>();
        public DbSet<ComponentStockCountModel> ComponentStockCounts => Set<ComponentStockCountModel>();
        public DbSet<DeviceComponent> DeviceComponents => Set<DeviceComponent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка TPT наследования
            modelBuilder.Entity<BaseModel>().ToTable("Products");
            modelBuilder.Entity<LiquidModel>().ToTable("Liquids");
            modelBuilder.Entity<DeviceModel>().ToTable("Devices");

            // Настройка отношений остатков
            ConfigureStockCount<LiquidStockCountModel, LiquidModel>(modelBuilder, "LiquidStockCounts");
            ConfigureStockCount<ComponentStockCountModel, ComponentModel>(modelBuilder, "ComponentStockCounts");
            ConfigureStockCount<DeviceStockCountModel, DeviceModel>(modelBuilder, "DeviceStockCounts");

            // Отношение многие-ко-многим Device-Component
            modelBuilder.Entity<DeviceComponent>()
                .HasKey(dc => new { dc.DeviceId, dc.ComponentId });

            modelBuilder.Entity<DeviceComponent>()
                .HasOne(dc => dc.Device)
                .WithMany(d => d.CompatibleComponents)
                .HasForeignKey(dc => dc.DeviceId);

            modelBuilder.Entity<DeviceComponent>()
                .HasOne(dc => dc.Component)
                .WithMany(c => c.CompatibleDevices)
                .HasForeignKey(dc => dc.ComponentId);

            // Настройка отношения Manufacturer
            modelBuilder.Entity<BaseModel>()
                .HasOne(b => b.Manufacturer)
                .WithMany() // Убрали навигацию с обратной стороны
                .HasForeignKey(b => b.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict); // Измените на Cascade при необходимости
        }

        private void ConfigureStockCount<TStock, TProduct>(ModelBuilder modelBuilder, string tableName)
            where TStock : class
            where TProduct : class
        {
            modelBuilder.Entity<TStock>()
                .ToTable(tableName)
                .HasOne<TProduct>()
                .WithMany()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
