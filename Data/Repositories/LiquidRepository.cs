using EShop.API.Data.Repositories.Interfaces;
using EShop.API.Models.Liquids;
using Microsoft.EntityFrameworkCore;

namespace EShop.API.Data.Repositories
{
    public class LiquidRepository : ILiquidRepository
    {
        private readonly AppDbContext _context;

        public LiquidRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<LiquidModel>> GetAllAsync()
        {
            return await _context.Liquids.ToListAsync();
        }

        public async Task<LiquidModel?> GetByIdAsync(int id)
        {
            return await _context.Liquids.FindAsync(id);
        }

        public async Task<LiquidModel> CreateAsync(LiquidModel liquid)
        {
            _context.Liquids.Add(liquid);
            await _context.SaveChangesAsync();
            return liquid;
        }

        public async Task UpdateAsync(LiquidModel liquid)
        {
            _context.Entry(liquid).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var liquid = await _context.Liquids.FindAsync(id);
            if (liquid != null)
            {
                _context.Liquids.Remove(liquid);
                await _context.SaveChangesAsync();
            }
        }
    }
}
