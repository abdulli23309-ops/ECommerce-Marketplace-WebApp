using System;

namespace ECommerce.Domain.Entities
{
    public class RolePermissionGroup
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
        
        public ICollection<RolePermissionGroup> RolePermissionGroups { get; set; } = new List<RolePermissionGroup>();
        public Guid PermissionGroupId { get; set; }
        public PermissionGroup PermissionGroup { get; set; } = null!;
    }
}