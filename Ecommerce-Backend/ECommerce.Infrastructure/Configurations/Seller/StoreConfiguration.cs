using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Seller
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable("Stores");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
            builder.Property(s => s.Description).HasMaxLength(1000);
            builder.Property(s => s.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(s => s.IsDeleted).IsRequired().HasDefaultValue(false);
            builder.Property(s => s.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasOne(s => s.SellerProfile)
                   .WithMany(sp => sp.Stores)
                   .HasForeignKey(s => s.SellerProfileId)
                   .OnDelete(DeleteBehavior.Restrict); // prevent seller deletion if stores exist

            builder.HasQueryFilter(s => !s.IsDeleted); // global filter for soft delete
        }
    }
}