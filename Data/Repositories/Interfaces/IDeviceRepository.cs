using EShop.API.Models.Devices;

namespace EShop.API.Data.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        Task<List<DeviceModel>> GetAllAsync();
        Task<DeviceModel?> GetByIdAsync(int id);
        Task<DeviceModel> CreateAsync(DeviceModel device);
        Task UpdateAsync(DeviceModel device);
        Task DeleteAsync(int id);
        Task AddComponentAsync(int deviceId, int componentId);
        Task RemoveComponentAsync(int deviceId, int componentId);
    }
}
