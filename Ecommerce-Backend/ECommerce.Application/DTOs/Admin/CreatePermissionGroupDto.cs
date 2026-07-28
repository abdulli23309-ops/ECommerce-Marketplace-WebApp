namespace ECommerce.Application.DTOs.Admin
{
    public class CreatePermissionGroupDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<Guid>? PermissionIds { get; set; }
    }
}