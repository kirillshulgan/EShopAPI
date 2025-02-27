using EShop.API.Data.Repositories.Interfaces;
using EShop.API.Models.Manufacturers;
using Microsoft.EntityFrameworkCore;

namespace EShop.API.Data.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly AppDbContext _context;

        public ManufacturerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ManufacturerModel>> GetAllAsync()
        {
            return await _context.Manufacturers.ToListAsync();
        }

        public async Task<ManufacturerModel?> GetByIdAsync(int id)
        {
            return await _context.Manufacturers.FindAsync(id);
        }

        public async Task<ManufacturerModel> CreateAsync(ManufacturerModel manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();
            return manufacturer;
        }

        public async Task UpdateAsync(ManufacturerModel manufacturer)
        {
            _context.Entry(manufacturer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer != null)
            {
                _context.Manufacturers.Remove(manufacturer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _context.Manufacturers
                .AnyAsync(m => m.Name.ToLower() == name.ToLower());
        }
    }
}
