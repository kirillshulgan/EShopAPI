using System.ComponentModel.DataAnnotations.Schema;
using EShop.API.Models.Manufacturers;

namespace EShop.API.Models
{
    public class BaseModel
    {
        //Идентификатор
        public int Id { get; set; }

        //Наименование
        public string Name { get; set; }

        //Описание
        public string Description { get; set; }

        //ID производителя
        public int ManufacturerId { get; set; }

        //Производитель
        public ManufacturerModel Manufacturer { get; set; }

        //Стоимость
        public decimal Price { get; set; }        

        //Изображение
        public byte[] Image { get; set; }
    }
}
