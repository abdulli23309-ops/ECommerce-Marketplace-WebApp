using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Orders
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipments");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            builder.Property(s => s.TrackingNumber).HasMaxLength(200);
            builder.Property(s => s.Carrier).HasMaxLength(100);
            builder.Property(s => s.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasIndex(s => s.SellerOrderId).IsUnique(); // one shipment per seller order

            builder.HasOne(s => s.SellerOrder)
                   .WithOne() // no navigation back to Shipment yet; can add later
                   .HasForeignKey<Shipment>(s => s.SellerOrderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}