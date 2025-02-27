using EShop.API.Models.Manufacturers;

namespace EShop.API.Data.Repositories.Interfaces
{
    public interface IManufacturerRepository
    {
        Task<IEnumerable<ManufacturerModel>> GetAllAsync();
        Task<ManufacturerModel?> GetByIdAsync(int id);
        Task<ManufacturerModel> CreateAsync(ManufacturerModel manufacturer);
        Task UpdateAsync(ManufacturerModel manufacturer);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(string name);
    }
}
