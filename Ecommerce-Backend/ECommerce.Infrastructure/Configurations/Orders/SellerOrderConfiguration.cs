using ECommerce.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ECommerce.Infrastructure.Data.Configurations.Orders
{
    public class SellerOrderConfiguration : IEntityTypeConfiguration<SellerOrder>
    {
        public void Configure(EntityTypeBuilder<SellerOrder> builder)
        {
            builder.ToTable("SellerOrders");
            builder.HasKey(so => so.Id);

            builder.Property(so => so.SubTotal).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(so => so.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            builder.Property(so => so.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasOne(so => so.Store)
                   .WithMany(s => s.SellerOrders)
                   .HasForeignKey(so => so.StoreId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(so => so.OrderItems)
                   .WithOne(oi => oi.SellerOrder)
                   .HasForeignKey(oi => oi.SellerOrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


