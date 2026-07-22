using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Catalog
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.ImageUrl).IsRequired().HasMaxLength(500);
            builder.Property(pi => pi.SortOrder).IsRequired().HasDefaultValue(0);
            builder.Property(pi => pi.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");
            builder.HasQueryFilter(pi => pi.Product.IsDeleted == false);
            builder.HasOne(pi => pi.Product)
                   .WithMany(p => p.ProductImages)
                   .HasForeignKey(pi => pi.ProductId)
                   .OnDelete(DeleteBehavior.Cascade); // images removed when product is deleted
        }
    }
}