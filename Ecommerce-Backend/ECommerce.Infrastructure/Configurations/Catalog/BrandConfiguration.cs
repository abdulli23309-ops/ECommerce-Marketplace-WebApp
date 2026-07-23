using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Catalog
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name).IsRequired().HasMaxLength(200);
            builder.HasIndex(b => b.Name).IsUnique();
            builder.Property(b => b.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(b => b.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");
            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}