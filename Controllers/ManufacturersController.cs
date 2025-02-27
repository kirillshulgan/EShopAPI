using EShop.API.Models.Manufacturers;
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
    [SwaggerTag("Управление производителями")]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerService _service;
        private readonly ILogger<ManufacturersController> _logger;

        public ManufacturersController(
            IManufacturerService service,
            ILogger<ManufacturersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Получить список всех производителей
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ManufacturerModel>), 200)]
        [SwaggerOperation(Summary = "Получение списка производителей")]
        public async Task<ActionResult<IEnumerable<ManufacturerModel>>> GetAll()
        {
            try
            {
                var manufacturers = await _service.GetAllManufacturersAsync();
                return Ok(manufacturers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting manufacturers");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получить производителя по ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ManufacturerModel), 200)]
        [ProducesResponseType(404)]
        [SwaggerOperation(Summary = "Получение производителя по ID")]
        public async Task<ActionResult<ManufacturerModel>> GetById(
            [Range(1, int.MaxValue)] int id)
        {
            try
            {
                var manufacturer = await _service.GetManufacturerByIdAsync(id);
                return Ok(manufacturer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting manufacturer with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Создать нового производителя
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ManufacturerModel), 201)]
        [SwaggerOperation(Summary = "Создание нового производителя")]
        public async Task<ActionResult<ManufacturerModel>> Create(
            [FromBody, SwaggerRequestBody("Данные производителя", Required = true)] ManufacturerModel manufacturer)
        {
            try
            {
                var createdManufacturer = await _service.CreateManufacturerAsync(manufacturer);
                return CreatedAtAction(nameof(GetById), new { id = createdManufacturer.Id }, createdManufacturer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating manufacturer");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Обновить производителя
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Summary = "Обновление производителя")]
        public async Task<IActionResult> Update(
            [Range(1, int.MaxValue)] int id,
            [FromBody] ManufacturerModel manufacturer)
        {
            if (id != manufacturer.Id)
                return BadRequest("ID mismatch");

            try
            {
                await _service.UpdateManufacturerAsync(manufacturer);
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
                _logger.LogError(ex, $"Error updating manufacturer with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Удалить производителя
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [SwaggerOperation(Summary = "Удаление производителя")]
        public async Task<IActionResult> Delete(
            [Range(1, int.MaxValue)] int id)
        {
            try
            {
                await _service.DeleteManufacturerAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting manufacturer with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
