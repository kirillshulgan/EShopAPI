using EShop.API.Models.Components;

namespace EShop.API.Data.Repositories.Interfaces
{
    public interface IComponentRepository
    {
        Task<List<ComponentModel>> GetAllAsync();
        Task<ComponentModel?> GetByIdAsync(int id);
        Task<ComponentModel> CreateAsync(ComponentModel component);
        Task UpdateAsync(ComponentModel component);
        Task DeleteAsync(int id);
        Task AddCompatibleDeviceAsync(int componentId, int deviceId);
        Task RemoveCompatibleDeviceAsync(int componentId, int deviceId);
    }
}
