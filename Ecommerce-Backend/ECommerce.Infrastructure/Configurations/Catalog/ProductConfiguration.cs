using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Catalog
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(300);
            builder.HasIndex(p => p.Name);

            builder.Property(p => p.BasePrice).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.Status).IsRequired().HasMaxLength(50).HasDefaultValue("PendingApproval");
            builder.Property(p => p.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasOne(p => p.Store)
                   .WithMany(s => s.Products)
                   .HasForeignKey(p => p.StoreId)
                   .OnDelete(DeleteBehavior.Restrict); // cannot delete store if products exist

            builder.HasOne(p => p.SubCategory)
                   .WithMany(sc => sc.Products)
                   .HasForeignKey(p => p.SubCategoryId)
                   .OnDelete(DeleteBehavior.SetNull); // subcategory deletion sets FK to null

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}