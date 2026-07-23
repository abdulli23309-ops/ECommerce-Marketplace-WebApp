namespace ECommerce.Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; } = null!;
        public Guid? ProductId { get; set; }         
        public Product? Product { get; set; }         
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ReviewImage> ReviewImages { get; set; } = new List<ReviewImage>();
    }
}