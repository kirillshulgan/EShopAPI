using EShop.API.Data.Repositories.Interfaces;
using EShop.API.Models.Manufacturers;
using EShop.API.Services.Interfaces;

namespace EShop.API.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _repository;
        private readonly ILogger<ManufacturerService> _logger;

        public ManufacturerService(
            IManufacturerRepository repository,
            ILogger<ManufacturerService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<ManufacturerModel>> GetAllManufacturersAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all manufacturers");
                throw;
            }
        }

        public async Task<ManufacturerModel?> GetManufacturerByIdAsync(int id)
        {
            var manufacturer = await _repository.GetByIdAsync(id);
            return manufacturer ?? throw new KeyNotFoundException($"Manufacturer with id {id} not found");
        }

        public async Task<ManufacturerModel> CreateManufacturerAsync(ManufacturerModel manufacturer)
        {
            ValidateManufacturer(manufacturer);

            if (await _repository.ExistsAsync(manufacturer.Name))
                throw new ArgumentException("Manufacturer with this name already exists");

            return await _repository.CreateAsync(manufacturer);
        }

        public async Task UpdateManufacturerAsync(ManufacturerModel manufacturer)
        {
            ValidateManufacturer(manufacturer);

            var existing = await _repository.GetByIdAsync(manufacturer.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Manufacturer with id {manufacturer.Id} not found");

            if (await _repository.ExistsAsync(manufacturer.Name) && existing.Name != manufacturer.Name)
                throw new ArgumentException("Manufacturer with this name already exists");

            await _repository.UpdateAsync(manufacturer);
        }

        public async Task DeleteManufacturerAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private void ValidateManufacturer(ManufacturerModel manufacturer)
        {
            if (string.IsNullOrWhiteSpace(manufacturer.Name))
                throw new ArgumentException("Name is required");

            if (manufacturer.Name.Length > 100)
                throw new ArgumentException("Name cannot exceed 100 characters");
        }
    }
}
