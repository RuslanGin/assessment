using Shared.Models;

namespace BusinessLayer.Models
{
    public class OrderBl
    {
        // assume that data are valid and all lines have 10 fields
        public OrderBl(string[] values)
        {
            Key = values[0];
            ArticleCode = values[1];
            ColorCode = values[2];
            Description = values[3];
            Price = values[4];
            DiscountPrice = values[5];
            DeliveredIn = values[6];
            Q1 = values[7];
            Size = values[8];
            Color = values[9];
        }

        public string Key { get; set; }
        public string ArticleCode { get; set; }
        public string ColorCode { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string DiscountPrice { get; set; }
        public string DeliveredIn { get; set; }
        public string Q1 { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }

        public Order ToEntity()
        {
            return new Order
            {
                Key = Key,
                ArticleCode = ArticleCode,
                Color = Color,
                ColorCode = ColorCode,
                Description = Description,
                Price = Price,
                DiscountPrice = DiscountPrice,
                DeliveredIn = DeliveredIn,
                Q1 = Q1,
                Size = Size
            };
        } 
    }
}
