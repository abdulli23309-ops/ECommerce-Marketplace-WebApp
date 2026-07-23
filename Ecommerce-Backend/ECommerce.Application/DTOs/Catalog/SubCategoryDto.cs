namespace ECommerce.Application.DTOs.Catalog
{
    public class SubCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
    }
}