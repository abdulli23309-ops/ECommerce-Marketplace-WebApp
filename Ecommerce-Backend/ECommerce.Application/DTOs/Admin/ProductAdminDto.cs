namespace ECommerce.Application.DTOs.Admin
{
    public class ProductAdminDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsDeleted { get; set; }
    }
}