using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Permissions
{
    public class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
    {
        public void Configure(EntityTypeBuilder<PermissionGroup> builder)
        {
            builder.ToTable("PermissionGroups");
            builder.HasKey(pg => pg.Id);

            builder.Property(pg => pg.Name).IsRequired().HasMaxLength(200);
            builder.HasIndex(pg => pg.Name).IsUnique();

            builder.Property(pg => pg.Description).HasMaxLength(500);
        }
    }
}