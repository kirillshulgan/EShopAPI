using EShop.API.Models.Liquids;

namespace EShop.API.Services.Interfaces
{
    public interface ILiquidService
    {
        Task<List<LiquidModel>> GetAllLiquidsAsync();
        Task<LiquidModel?> GetLiquidByIdAsync(int id);
        Task<LiquidModel> CreateLiquidAsync(LiquidModel liquid);
        Task UpdateLiquidAsync(LiquidModel liquid);
        Task DeleteLiquidAsync(int id);
    }
}
