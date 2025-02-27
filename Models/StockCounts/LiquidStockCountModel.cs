using EShop.API.Models.Liquids;

namespace EShop.API.Models.StockCounts
{
    public class LiquidStockCountModel
    {
        //Идентификатор
        public int Id { get; set; }

        //Наименование склада
        public string Name { get; set; }

        //Количество
        public int Count { get; set; }

        //ID жидкости
        public int LiquidId { get; set; }

        //Жидкость
        public LiquidModel Liquid { get; set; }
    }
}
