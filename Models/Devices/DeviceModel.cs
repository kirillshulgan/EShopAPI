namespace EShop.API.Models.Devices
{
    public class DeviceModel : BaseModel
    {
        //Тип (ПодСистема, Одноразка, МехМод)
        public string Type { get; set; }

        //Максимальная мощность
        public int MaxPower { get; set; }

        //Объем родного картриджа
        public int Volume { get; set; }

        //Гарантия в месяцах
        public int Guarantee { get; set; }

        //Подходящие компоненты
        public List<DeviceComponent> CompatibleComponents { get; set; }
    }
}
