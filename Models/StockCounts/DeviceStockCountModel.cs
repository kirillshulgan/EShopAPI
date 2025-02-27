using EShop.API.Models.Devices;

namespace EShop.API.Models.StockCounts
{
    public class DeviceStockCountModel
    {
        //Идентификатор
        public int Id { get; set; }

        //Наименование склада
        public string Name { get; set; }

        //Количество
        public int Count { get; set; }

        //ID устройства
        public int DeviceId { get; set; }

        //Устройство
        public DeviceModel Device { get; set; }
    }
}
