using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Catalog
{
    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.ToTable("SubCategories");
            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.Name).IsRequired().HasMaxLength(200);
            builder.Property(sc => sc.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(sc => sc.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasOne(sc => sc.Category)
                   .WithMany(c => c.SubCategories)
                   .HasForeignKey(sc => sc.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict); // prevent category deletion if subcategories exist

            builder.HasQueryFilter(sc => !sc.IsDeleted);
        }
    }
}