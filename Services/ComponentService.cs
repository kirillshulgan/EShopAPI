using EShop.API.Data.Repositories.Interfaces;
using EShop.API.Models.Components;
using EShop.API.Services.Interfaces;

namespace EShop.API.Services
{
    public class ComponentService : IComponentService
    {
        private readonly IComponentRepository _repository;
        private readonly ILogger<ComponentService> _logger;

        public ComponentService(
            IComponentRepository repository,
            ILogger<ComponentService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<ComponentModel>> GetAllComponentsAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all components");
                throw;
            }
        }

        public async Task<ComponentModel?> GetComponentByIdAsync(int id)
        {
            var component = await _repository.GetByIdAsync(id);
            return component ?? throw new KeyNotFoundException($"Component with id {id} not found");
        }

        public async Task<ComponentModel> CreateComponentAsync(ComponentModel component)
        {
            ValidateComponent(component);
            return await _repository.CreateAsync(component);
        }

        public async Task UpdateComponentAsync(ComponentModel component)
        {
            ValidateComponent(component);
            await _repository.UpdateAsync(component);
        }

        public async Task DeleteComponentAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task AddCompatibleDeviceAsync(int componentId, int deviceId)
        {
            try
            {
                await _repository.AddCompatibleDeviceAsync(componentId, deviceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding device {deviceId} to component {componentId}");
                throw;
            }
        }

        public async Task RemoveCompatibleDeviceAsync(int componentId, int deviceId)
        {
            try
            {
                await _repository.RemoveCompatibleDeviceAsync(componentId, deviceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing device {deviceId} from component {componentId}");
                throw;
            }
        }

        private void ValidateComponent(ComponentModel component)
        {
            if (component.Price <= 0)
                throw new ArgumentException("Price must be greater than zero");

            if (string.IsNullOrWhiteSpace(component.Name))
                throw new ArgumentException("Name is required");

            if (component.Volume <= 0)
                throw new ArgumentException("Volume must be positive");
        }
    }
}
