namespace EShop.API.Controllers
{
    using EShop.API.Models.Devices;
    using EShop.API.Services.Interfaces;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Управление устройствами")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _service;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(
            IDeviceService service,
            ILogger<DevicesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Получить список всех устройств
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<DeviceModel>), 200)]
        [SwaggerOperation(Summary = "Получение списка устройств")]
        public async Task<ActionResult<IEnumerable<DeviceModel>>> GetAll()
        {
            try
            {
                var devices = await _service.GetAllDevicesAsync();
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting devices");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получить устройство по ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(DeviceModel), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Получение устройства по ID")]
        public async Task<ActionResult<DeviceModel>> GetById(
            [Range(1, int.MaxValue)] int id)
        {
            try
            {
                var device = await _service.GetDeviceByIdAsync(id);
                return Ok(device);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting device with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Создать новое устройство
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(DeviceModel), 201)]
        [SwaggerOperation(Summary = "Создание нового устройства")]
        public async Task<ActionResult<DeviceModel>> Create(
            [FromBody, SwaggerRequestBody("Данные устройства", Required = true)] DeviceModel device)
        {
            try
            {
                var createdDevice = await _service.CreateDeviceAsync(device);
                return CreatedAtAction(nameof(GetById), new { id = createdDevice.Id }, createdDevice);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating device");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Обновить устройство
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Summary = "Обновление устройства")]
        public async Task<IActionResult> Update(
            [Range(1, int.MaxValue)] int id,
            [FromBody] DeviceModel device)
        {
            if (id != device.Id)
                return BadRequest("ID mismatch");

            try
            {
                await _service.UpdateDeviceAsync(device);
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
                _logger.LogError(ex, $"Error updating device with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Удалить устройство
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Summary = "Удаление устройства")]
        public async Task<IActionResult> Delete(
            [Range(1, int.MaxValue)] int id)
        {
            try
            {
                await _service.DeleteDeviceAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting device with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Добавить компонент к устройству
        /// </summary>
        [HttpPost("{deviceId}/components/{componentId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Summary = "Добавление компонента к устройству")]
        public async Task<IActionResult> AddComponent(
            [Range(1, int.MaxValue)] int deviceId,
            [Range(1, int.MaxValue)] int componentId)
        {
            try
            {
                await _service.AddComponentToDeviceAsync(deviceId, componentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding component {componentId} to device {deviceId}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Удалить компонент из устройства
        /// </summary>
        [HttpDelete("{deviceId}/components/{componentId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Summary = "Удаление компонента из устройства")]
        public async Task<IActionResult> RemoveComponent(
            [Range(1, int.MaxValue)] int deviceId,
            [Range(1, int.MaxValue)] int componentId)
        {
            try
            {
                await _service.RemoveComponentFromDeviceAsync(deviceId, componentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing component {componentId} from device {deviceId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
