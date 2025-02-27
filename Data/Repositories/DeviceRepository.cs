using EShop.API.Data.Repositories.Interfaces;
using EShop.API.Models.Devices;
using EShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.API.Data.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly AppDbContext _context;

        public DeviceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DeviceModel>> GetAllAsync()
        {
            return await _context.Devices
                .Include(d => d.CompatibleComponents)
                .ThenInclude(dc => dc.Component)
                .ToListAsync();
        }

        public async Task<DeviceModel?> GetByIdAsync(int id)
        {
            return await _context.Devices
                .Include(d => d.CompatibleComponents)
                .ThenInclude(dc => dc.Component)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DeviceModel> CreateAsync(DeviceModel device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task UpdateAsync(DeviceModel device)
        {
            _context.Entry(device).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddComponentAsync(int deviceId, int componentId)
        {
            var deviceComponent = new DeviceComponent
            {
                DeviceId = deviceId,
                ComponentId = componentId
            };

            _context.DeviceComponents.Add(deviceComponent);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveComponentAsync(int deviceId, int componentId)
        {
            var deviceComponent = await _context.DeviceComponents
                .FirstOrDefaultAsync(dc => dc.DeviceId == deviceId && dc.ComponentId == componentId);

            if (deviceComponent != null)
            {
                _context.DeviceComponents.Remove(deviceComponent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
