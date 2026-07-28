using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Catalog
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
            builder.HasIndex(c => c.Name).IsUnique().HasFilter("[IsDeleted] = 0");

            builder.Property(c => c.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(c => c.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}