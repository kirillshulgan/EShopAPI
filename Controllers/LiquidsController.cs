using EShop.API.Models.Liquids;
using EShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EShop.API.Controllers
{
    /// <summary>
    /// Контроллер для работы с жидкостями
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [SwaggerTag("Управление жидкостями для электронных сигарет")]
    public class LiquidsController : ControllerBase
    {
        private readonly ILiquidService _service;
        private readonly ILogger<LiquidsController> _logger;

        public LiquidsController(
            ILiquidService service,
            ILogger<LiquidsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Получить список всех жидкостей
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        /// GET /api/liquids
        /// </remarks>
        /// <response code="200">Возвращает список жидкостей</response>
        /// <response code="500">Произошла внутренняя ошибка сервера</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<LiquidModel>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Получение списка жидкостей",
            Description = "Возвращает полный список доступных жидкостей",
            OperationId = "GetAllLiquids")]
        public async Task<ActionResult<IEnumerable<LiquidModel>>> GetAll()
        {
            try
            {
                var liquids = await _service.GetAllLiquidsAsync();
                return Ok(liquids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting liquids");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Получить жидкость по ID
        /// </summary>
        /// <param name="id" example="1">Идентификатор жидкости</param>
        /// <remarks>
        /// Пример запроса:
        /// GET /api/liquids/1
        /// </remarks>
        /// <response code="200">Возвращает запрашиваемую жидкость</response>
        /// <response code="404">Жидкость не найдена</response>
        /// <response code="500">Произошла внутренняя ошибка сервера</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(LiquidModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Получение жидкости по ID",
            Description = "Возвращает детальную информацию о конкретной жидкости",
            OperationId = "GetLiquidById")]
        public async Task<ActionResult<LiquidModel>> GetById(int id)
        {
            try
            {
                var liquid = await _service.GetLiquidByIdAsync(id);
                return Ok(liquid);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting liquid with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Создать новую жидкость
        /// </summary>
        /// <param name="liquid">Данные новой жидкости</param>
        /// <remarks>
        /// Пример запроса:
        /// POST /api/liquids
        /// {
        ///     "name": "Мятная свежесть",
        ///     "description": "Освежающий мятный вкус",
        ///     "manufacturerId": 1,
        ///     "price": 15.99,
        ///     "type": "Солевая",
        ///     "volume": 30,
        ///     "strength": 20,
        ///     "vgpg": "50/50"
        /// }
        /// </remarks>
        /// <response code="201">Жидкость успешно создана</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="500">Произошла внутренняя ошибка сервера</response>
        [HttpPost]
        [Authorize(Roles = "Admin")] // Только админы
        [ProducesResponseType(typeof(LiquidModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Создание новой жидкости",
            Description = "Добавляет новую жидкость в каталог",
            OperationId = "CreateLiquid")]
        [SwaggerRequestExample(typeof(LiquidModel), typeof(LiquidModelExample))]
        [HttpPost]
        public async Task<ActionResult<LiquidModel>> Create([FromBody] LiquidModel liquid)
        {
            try
            {
                var createdLiquid = await _service.CreateLiquidAsync(liquid);
                return CreatedAtAction(nameof(GetById), new { id = createdLiquid.Id }, createdLiquid);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating liquid");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Обновить существующую жидкость
        /// </summary>
        /// <param name="id">Идентификатор жидкости</param>
        /// <param name="liquid">Обновленные данные жидкости</param>
        /// <remarks>
        /// Пример запроса:
        /// PUT /api/liquids/1
        /// {
        ///     "id": 1,
        ///     "name": "Мятная свежесть Pro",
        ///     "price": 17.99
        /// }
        /// </remarks>
        /// <response code="204">Жидкость успешно обновлена</response>
        /// <response code="400">Некорректные входные данные</response>
        /// <response code="404">Жидкость не найдена</response>
        /// <response code="500">Произошла внутренняя ошибка сервера</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Только админы
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Обновление жидкости",
            Description = "Обновляет данные существующей жидкости",
            OperationId = "UpdateLiquid")]
        public async Task<IActionResult> Update(int id, [FromBody] LiquidModel liquid)
        {
            if (id != liquid.Id)
                return BadRequest("ID mismatch");

            try
            {
                await _service.UpdateLiquidAsync(liquid);
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
                _logger.LogError(ex, $"Error updating liquid with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Удалить жидкость
        /// </summary>
        /// <param name="id" example="1">Идентификатор жидкости</param>
        /// <remarks>
        /// Пример запроса:
        /// DELETE /api/liquids/1
        /// </remarks>
        /// <response code="204">Жидкость успешно удалена</response>
        /// <response code="404">Жидкость не найдена</response>
        /// <response code="500">Произошла внутренняя ошибка сервера</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Только админы
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(
            Summary = "Удаление жидкости",
            Description = "Удаляет жидкость из каталога по ID",
            OperationId = "DeleteLiquid")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteLiquidAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting liquid with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Пример модели жидкости
        /// </summary>
        public class LiquidModelExample : IExamplesProvider<LiquidModel>
        {
            public LiquidModel GetExamples()
            {
                return new LiquidModel
                {
                    Id = 1,
                    Name = "Пример жидкости",
                    Description = "Пример описания жидкости",
                    ManufacturerId = 1,
                    Price = 19.99m,
                    Type = "Солевая",
                    Volume = 30,
                    Strength = 20,
                    VGPG = "50/50"
                };
            }
        }
    }
}
