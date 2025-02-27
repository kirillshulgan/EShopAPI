namespace EShop.API.Models.Components
{
    public class ComponentModel : BaseModel
    {
        //Тип (Картридж, Испаритель)
        public string Type { get; set; }

        //Объем мл
        public int Volume { get; set; }

        //Совместимые устройства
        public List<DeviceComponent> CompatibleDevices { get; set; }
    }
}
