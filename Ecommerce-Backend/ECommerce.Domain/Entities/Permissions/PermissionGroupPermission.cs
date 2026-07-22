namespace ECommerce.Domain.Entities
{
    public class PermissionGroupPermission
    {
        public Guid PermissionGroupId { get; set; }
        public PermissionGroup PermissionGroup { get; set; } = null!;

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}