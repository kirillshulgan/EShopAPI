using EShop.API.Models.Liquids;

namespace EShop.API.Data.Repositories.Interfaces
{
    public interface ILiquidRepository
    {
        Task<List<LiquidModel>> GetAllAsync();
        Task<LiquidModel?> GetByIdAsync(int id);
        Task<LiquidModel> CreateAsync(LiquidModel liquid);
        Task UpdateAsync(LiquidModel liquid);
        Task DeleteAsync(int id);
    }
}
