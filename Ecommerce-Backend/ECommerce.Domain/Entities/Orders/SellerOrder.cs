

namespace ECommerce.Domain.Entities
{
    public class SellerOrder
    {
        public Guid Id { get; set; }
        public Guid ParentOrderId { get; set; }
        public ParentOrder ParentOrder { get; set; } = null!;
        public Guid? StoreId { get; set; }
        public Store? Store { get; set; }
        public decimal SubTotal { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}