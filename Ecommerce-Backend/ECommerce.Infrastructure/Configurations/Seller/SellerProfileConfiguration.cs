using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Seller
{
    public class SellerProfileConfiguration : IEntityTypeConfiguration<SellerProfile>
    {
        public void Configure(EntityTypeBuilder<SellerProfile> builder)
        {
            builder.ToTable("SellerProfiles");
            builder.HasKey(sp => sp.Id);

            builder.Property(sp => sp.BusinessName).IsRequired().HasMaxLength(200);
            builder.Property(sp => sp.Description).HasMaxLength(1000);
            builder.Property(sp => sp.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            builder.Property(sp => sp.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Property(sp => sp.RejectionReason).HasMaxLength(1000);

            builder.HasOne(sp => sp.User)
                   .WithOne(u => u.SellerProfile)
                   .HasForeignKey<SellerProfile>(sp => sp.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}