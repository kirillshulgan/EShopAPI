using EShop.API.Models.Components;
using EShop.API.Models.Devices;

namespace EShop.API.Models
{
    public class DeviceComponent
    {
        public int DeviceId { get; set; }
        public DeviceModel Device { get; set; }

        public int ComponentId { get; set; }
        public ComponentModel Component { get; set; }
    }
}
