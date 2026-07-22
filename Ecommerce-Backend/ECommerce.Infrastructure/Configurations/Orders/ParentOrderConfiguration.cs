using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Orders
{
    public class ParentOrderConfiguration : IEntityTypeConfiguration<ParentOrder>
    {
        public void Configure(EntityTypeBuilder<ParentOrder> builder)
        {
            builder.ToTable("ParentOrders");
            builder.HasKey(po => po.Id);

            builder.Property(po => po.OrderDate).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Property(po => po.OrderStatus).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            builder.Property(po => po.ShippingFullName).IsRequired().HasMaxLength(200);
            builder.Property(po => po.ShippingPhone).IsRequired().HasMaxLength(20);
            builder.Property(po => po.ShippingAddressLine1).IsRequired().HasMaxLength(300);
            builder.Property(po => po.ShippingAddressLine2).HasMaxLength(300);
            builder.Property(po => po.ShippingCity).IsRequired().HasMaxLength(100);
            builder.Property(po => po.ShippingState).HasMaxLength(100);
            builder.Property(po => po.ShippingPostalCode).HasMaxLength(20);
            builder.Property(po => po.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(po => po.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasOne(po => po.Customer)
                   .WithMany(u => u.ParentOrders)
                   .HasForeignKey(po => po.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(po => po.SellerOrders)
                   .WithOne(so => so.ParentOrder)
                   .HasForeignKey(so => so.ParentOrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}