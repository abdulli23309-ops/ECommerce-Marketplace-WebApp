
namespace ECommerce.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public ICollection<RolePermissionGroup> RolePermissionGroups { get; set; } = new List<RolePermissionGroup>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}