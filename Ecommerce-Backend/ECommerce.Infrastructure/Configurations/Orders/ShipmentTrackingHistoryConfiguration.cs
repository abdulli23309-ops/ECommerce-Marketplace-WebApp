using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Orders
{
    public class ShipmentTrackingHistoryConfiguration : IEntityTypeConfiguration<ShipmentTrackingHistory>
    {
        public void Configure(EntityTypeBuilder<ShipmentTrackingHistory> builder)
        {
            builder.ToTable("ShipmentTrackingHistories");
            builder.HasKey(h => h.Id);
            builder.Property(h => h.Status).IsRequired().HasMaxLength(50);
            builder.Property(h => h.Location).HasMaxLength(300);
            builder.Property(h => h.Timestamp).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasOne(h => h.Shipment)
                   .WithMany(s => s.TrackingHistories)
                   .HasForeignKey(h => h.ShipmentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}