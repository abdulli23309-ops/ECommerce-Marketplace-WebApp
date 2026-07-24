namespace ECommerce.Domain.Entities
{
    public class ReturnImage
    {
        public Guid Id { get; set; }
        public Guid ReturnRequestId { get; set; }
        public ReturnRequest ReturnRequest { get; set; } = null!;
        public string ImageUrl { get; set; } = string.Empty;
    }
}