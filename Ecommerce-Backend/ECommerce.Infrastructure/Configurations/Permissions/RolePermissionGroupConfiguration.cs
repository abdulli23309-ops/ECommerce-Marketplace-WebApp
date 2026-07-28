using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations
{
    public class RolePermissionGroupConfiguration : IEntityTypeConfiguration<RolePermissionGroup>
    {
        public void Configure(EntityTypeBuilder<RolePermissionGroup> builder)
        {
            builder.ToTable("RolePermissionGroups");
            builder.HasKey(rpg => new { rpg.RoleId, rpg.PermissionGroupId });

            builder.HasOne(rpg => rpg.Role)
                   .WithMany(r => r.RolePermissionGroups)
                   .HasForeignKey(rpg => rpg.RoleId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rpg => rpg.PermissionGroup)
                   .WithMany(pg => pg.RolePermissionGroups)
                   .HasForeignKey(rpg => rpg.PermissionGroupId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}