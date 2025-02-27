using EShop.API.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [SwaggerTag("Управление ролями пользователей")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<RolesController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Создать новую роль
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                    return BadRequest("Role name is required");

                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

                return result.Succeeded
                    ? Ok()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Назначить роль пользователю
        /// </summary>
        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoleToUser(
            [FromBody] AssignRoleDto model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return NotFound("User not found");

                var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                return result.Succeeded
                    ? Ok()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Удалить роль у пользователя
        /// </summary>
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRoleFromUser(
            [FromBody] AssignRoleDto model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return NotFound("User not found");

                var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

                return result.Succeeded
                    ? Ok()
                    : BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing role");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class AssignRoleDto
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}
