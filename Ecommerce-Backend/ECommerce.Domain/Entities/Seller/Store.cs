

namespace ECommerce.Domain.Entities
{
    public class Store
    {
        public Guid Id { get; set; }
        public Guid SellerProfileId { get; set; }
        public SellerProfile SellerProfile { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? LogoUrl { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<SellerOrder> SellerOrders { get; set; } = new List<SellerOrder>();

    }
}