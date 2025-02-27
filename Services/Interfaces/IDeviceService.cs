using EShop.API.Models.Devices;

namespace EShop.API.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<List<DeviceModel>> GetAllDevicesAsync();
        Task<DeviceModel?> GetDeviceByIdAsync(int id);
        Task<DeviceModel> CreateDeviceAsync(DeviceModel device);
        Task UpdateDeviceAsync(DeviceModel device);
        Task DeleteDeviceAsync(int id);
        Task AddComponentToDeviceAsync(int deviceId, int componentId);
        Task RemoveComponentFromDeviceAsync(int deviceId, int componentId);
    }
}
