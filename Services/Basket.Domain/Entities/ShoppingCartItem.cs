using Basket.Domain.Interfaces;

namespace Basket.Domain.Entities
{
    public class ShoppingCardItem:IEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductStockQuantity { get; set; }
        public int Quantity { get; set; }
    }
}