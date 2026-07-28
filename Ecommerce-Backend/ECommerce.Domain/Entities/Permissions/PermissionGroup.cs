namespace ECommerce.Domain.Entities
{ 
    public class PermissionGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<RolePermissionGroup> RolePermissionGroups { get; set; } = new List<RolePermissionGroup>();
        public ICollection<PermissionGroupPermission> PermissionGroupPermissions { get; set; } = new List<PermissionGroupPermission>();
    }
}