using EShop.API.Models.Components;
using EShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Управление компонентами устройств")]
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentService _service;
        private readonly ILogger<ComponentsController> _logger;

        public ComponentsController(
            IComponentService service,
            ILogger<ComponentsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Получить список всех компонентов
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ComponentModel>), 200)]
        [SwaggerOperation(Summary = "Получение списка компонентов")]
        public async Task<ActionResult<IEnumerable<ComponentModel>>> GetAll()
        {
            try
            {
                var components = await _service.GetAllComponentsAsync();
                return Ok(components);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting components");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получить компонент по ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ComponentModel), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Получение компонента по ID")]
        public async Task<ActionResult<ComponentModel>> GetById(
            [Range(1, int.MaxValue)] int id)
        {
            try
            {
                var component = await _service.GetComponentByIdAsync(id);
                return Ok(component);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting component with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Создать новый компонент
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ComponentModel), 201)]
        [SwaggerOperation(Summary = "Создание нового компонента")]
        public async Task<ActionResult<ComponentModel>> Create(
            [FromBody, SwaggerRequestBody("Данные компонента", Required = true)] ComponentModel component)
        {
            try
            {
                var createdComponent = await _service.CreateComponentAsync(component);
                return CreatedAtAction(nameof(GetById), new { id = createdComponent.Id }, createdComponent);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating component");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Обновить компонент
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Summary = "Обновление компонента")]
        public async Task<IActionResult> Update(
            [Range(1, int.MaxValue)] int id,
            [FromBody] ComponentModel component)
        {
            if (id != component.Id)
                return BadRequest("ID mismatch");

            try
            {
                await _service.UpdateComponentAsync(component);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating component with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Удалить компонент
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Summary = "Удаление компонента")]
        public async Task<IActionResult> Delete(
            [Range(1, int.MaxValue)] int id)
        {
            try
            {
                await _service.DeleteComponentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting component with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Добавить совместимое устройство
        /// </summary>
        [HttpPost("{componentId}/devices/{deviceId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Summary = "Добавление совместимого устройства")]
        public async Task<IActionResult> AddDevice(
            [Range(1, int.MaxValue)] int componentId,
            [Range(1, int.MaxValue)] int deviceId)
        {
            try
            {
                await _service.AddCompatibleDeviceAsync(componentId, deviceId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding device {deviceId} to component {componentId}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Удалить совместимое устройство
        /// </summary>
        [HttpDelete("{componentId}/devices/{deviceId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Summary = "Удаление совместимого устройства")]
        public async Task<IActionResult> RemoveDevice(
            [Range(1, int.MaxValue)] int componentId,
            [Range(1, int.MaxValue)] int deviceId)
        {
            try
            {
                await _service.RemoveCompatibleDeviceAsync(componentId, deviceId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing device {deviceId} from component {componentId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
