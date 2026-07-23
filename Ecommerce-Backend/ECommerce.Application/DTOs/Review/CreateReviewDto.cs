namespace ECommerce.Application.DTOs.Reviews
{
    public class CreateReviewDto
    {
        public Guid OrderItemId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}