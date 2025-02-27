using EShop.API.Data.Repositories.Interfaces;
using EShop.API.Models.Liquids;
using EShop.API.Services.Interfaces;

namespace EShop.API.Services
{
    public class LiquidService : ILiquidService
    {
        private readonly ILiquidRepository _repository;
        private readonly ILogger<LiquidService> _logger;

        public LiquidService(
            ILiquidRepository repository,
            ILogger<LiquidService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<LiquidModel>> GetAllLiquidsAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all liquids");
                throw;
            }
        }

        public async Task<LiquidModel?> GetLiquidByIdAsync(int id)
        {
            var liquid = await _repository.GetByIdAsync(id);
            return liquid ?? throw new KeyNotFoundException($"Liquid with id {id} not found");
        }

        public async Task<LiquidModel> CreateLiquidAsync(LiquidModel liquid)
        {
            ValidateLiquid(liquid);
            return await _repository.CreateAsync(liquid);
        }

        public async Task UpdateLiquidAsync(LiquidModel liquid)
        {
            ValidateLiquid(liquid);
            await _repository.UpdateAsync(liquid);
        }

        public async Task DeleteLiquidAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private void ValidateLiquid(LiquidModel liquid)
        {
            if (liquid.Price <= 0)
                throw new ArgumentException("Price must be greater than zero");

            if (string.IsNullOrWhiteSpace(liquid.Name))
                throw new ArgumentException("Name is required");
        }
    }
}
