namespace ECommerce.Domain.Entities
{
    public class ReviewImage
    {
        public Guid Id { get; set; }
        public Guid ReviewId { get; set; }
        public Review Review { get; set; } = null!;
        public string ImageUrl { get; set; } = string.Empty;
    }
}