using EShop.API.Models.StockCounts;

namespace EShop.API.Models.Liquids
{
    public class LiquidModel : BaseModel
    {
        //Тип жидкости (никотин, соль, нулевка)
        public string Type { get; set; }

        //Объем банки
        public int Volume { get; set; }

        //Крепкость
        public int Strength { get; set; }

        //Соотношение компонентов
        public string VGPG { get; set; }

        //Остатки на складах
        public List<LiquidStockCountModel> StockCount { get; set; }
    }
}
