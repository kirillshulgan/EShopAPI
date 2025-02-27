using EShop.API.Models.Manufacturers;

namespace EShop.API.Services.Interfaces
{
    public interface IManufacturerService
    {
        Task<IEnumerable<ManufacturerModel>> GetAllManufacturersAsync();
        Task<ManufacturerModel?> GetManufacturerByIdAsync(int id);
        Task<ManufacturerModel> CreateManufacturerAsync(ManufacturerModel manufacturer);
        Task UpdateManufacturerAsync(ManufacturerModel manufacturer);
        Task DeleteManufacturerAsync(int id);
    }
}
