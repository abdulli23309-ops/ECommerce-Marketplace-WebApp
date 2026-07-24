using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Returns
{
    public class ReturnRequestConfiguration : IEntityTypeConfiguration<ReturnRequest>
    {
        public void Configure(EntityTypeBuilder<ReturnRequest> builder)
        {
            builder.ToTable("ReturnRequests");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Reason).IsRequired().HasMaxLength(1000);
            builder.Property(r => r.Description).HasMaxLength(2000);
            builder.Property(r => r.Status).IsRequired().HasConversion<int>();
            builder.Property(r => r.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            // Unique one‑to‑one with OrderItem
            builder.HasOne(r => r.OrderItem)
                   .WithOne()
                   .HasForeignKey<ReturnRequest>(r => r.OrderItemId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(r => r.OrderItemId).IsUnique();

            // One‑to‑many ReturnRequest → ReturnImages (cascade)
            builder.HasMany(r => r.ReturnImages)
                   .WithOne(ri => ri.ReturnRequest)
                   .HasForeignKey(ri => ri.ReturnRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}