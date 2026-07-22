using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Permissions
{
    public class PermissionGroupPermissionConfiguration : IEntityTypeConfiguration<PermissionGroupPermission>
    {
        public void Configure(EntityTypeBuilder<PermissionGroupPermission> builder)
        {
            builder.ToTable("PermissionGroupPermissions");
            builder.HasKey(pgp => new { pgp.PermissionGroupId, pgp.PermissionId });

            builder.HasOne(pgp => pgp.PermissionGroup)
                   .WithMany(pg => pg.PermissionGroupPermissions)
                   .HasForeignKey(pgp => pgp.PermissionGroupId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pgp => pgp.Permission)
                   .WithMany(p => p.PermissionGroupPermissions)
                   .HasForeignKey(pgp => pgp.PermissionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}