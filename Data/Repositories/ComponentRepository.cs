using EShop.API.Data.Repositories.Interfaces;
using EShop.API.Models.Components;
using EShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.API.Data.Repositories
{
    public class ComponentRepository : IComponentRepository
    {
        private readonly AppDbContext _context;

        public ComponentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ComponentModel>> GetAllAsync()
        {
            return await _context.Components
                .Include(c => c.CompatibleDevices)
                .ThenInclude(cd => cd.Device)
                .ToListAsync();
        }

        public async Task<ComponentModel?> GetByIdAsync(int id)
        {
            return await _context.Components
                .Include(c => c.CompatibleDevices)
                .ThenInclude(cd => cd.Device)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ComponentModel> CreateAsync(ComponentModel component)
        {
            _context.Components.Add(component);
            await _context.SaveChangesAsync();
            return component;
        }

        public async Task UpdateAsync(ComponentModel component)
        {
            _context.Entry(component).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var component = await _context.Components.FindAsync(id);
            if (component != null)
            {
                _context.Components.Remove(component);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddCompatibleDeviceAsync(int componentId, int deviceId)
        {
            var deviceComponent = new DeviceComponent
            {
                ComponentId = componentId,
                DeviceId = deviceId
            };

            _context.DeviceComponents.Add(deviceComponent);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCompatibleDeviceAsync(int componentId, int deviceId)
        {
            var deviceComponent = await _context.DeviceComponents
                .FirstOrDefaultAsync(dc => dc.ComponentId == componentId && dc.DeviceId == deviceId);

            if (deviceComponent != null)
            {
                _context.DeviceComponents.Remove(deviceComponent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
