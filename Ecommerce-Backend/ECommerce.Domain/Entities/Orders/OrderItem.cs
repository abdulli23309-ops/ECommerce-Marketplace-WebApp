

namespace ECommerce.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid SellerOrderId { get; set; }
        public SellerOrder SellerOrder { get; set; } = null!;
        public Guid? ProductId { get; set; }  // nullable, in case product is later deleted (set null)
        public Product? Product { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public decimal UnitPriceSnapshot { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}