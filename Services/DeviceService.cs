using EShop.API.Data.Repositories.Interfaces;
using EShop.API.Models.Devices;
using EShop.API.Services.Interfaces;

namespace EShop.API.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _repository;
        private readonly ILogger<DeviceService> _logger;

        public DeviceService(
            IDeviceRepository repository,
            ILogger<DeviceService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<DeviceModel>> GetAllDevicesAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all devices");
                throw;
            }
        }

        public async Task<DeviceModel?> GetDeviceByIdAsync(int id)
        {
            var device = await _repository.GetByIdAsync(id);
            return device ?? throw new KeyNotFoundException($"Device with id {id} not found");
        }

        public async Task<DeviceModel> CreateDeviceAsync(DeviceModel device)
        {
            ValidateDevice(device);
            return await _repository.CreateAsync(device);
        }

        public async Task UpdateDeviceAsync(DeviceModel device)
        {
            ValidateDevice(device);
            await _repository.UpdateAsync(device);
        }

        public async Task DeleteDeviceAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task AddComponentToDeviceAsync(int deviceId, int componentId)
        {
            try
            {
                await _repository.AddComponentAsync(deviceId, componentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding component {componentId} to device {deviceId}");
                throw;
            }
        }

        public async Task RemoveComponentFromDeviceAsync(int deviceId, int componentId)
        {
            try
            {
                await _repository.RemoveComponentAsync(deviceId, componentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing component {componentId} from device {deviceId}");
                throw;
            }
        }

        private void ValidateDevice(DeviceModel device)
        {
            if (device.Price <= 0)
                throw new ArgumentException("Price must be greater than zero");

            if (string.IsNullOrWhiteSpace(device.Name))
                throw new ArgumentException("Name is required");

            if (device.MaxPower < 0)
                throw new ArgumentException("Max power cannot be negative");
        }
    }
}
