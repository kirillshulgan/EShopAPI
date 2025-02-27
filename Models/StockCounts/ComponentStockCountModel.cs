using EShop.API.Models.Components;

namespace EShop.API.Models.StockCounts
{
    public class ComponentStockCountModel
    {
        //Идентификатор
        public int Id { get; set; }

        //Наименование склада
        public string Name { get; set; }

        //Количество
        public int Count { get; set; }

        //ID компонента
        public int ComponentId { get; set; }

        //Компонент
        public ComponentModel Component { get; set; }
    }
}
