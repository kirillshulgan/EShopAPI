using EShop.API.Models.Components;

namespace EShop.API.Services.Interfaces
{
    public interface IComponentService
    {
        Task<List<ComponentModel>> GetAllComponentsAsync();
        Task<ComponentModel?> GetComponentByIdAsync(int id);
        Task<ComponentModel> CreateComponentAsync(ComponentModel component);
        Task UpdateComponentAsync(ComponentModel component);
        Task DeleteComponentAsync(int id);
        Task AddCompatibleDeviceAsync(int componentId, int deviceId);
        Task RemoveCompatibleDeviceAsync(int componentId, int deviceId);
    }
}
