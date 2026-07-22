using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Payments
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.Method).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Status).IsRequired().HasMaxLength(50).HasDefaultValue("Pending");
            builder.Property(p => p.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasOne(p => p.ParentOrder)
                   .WithOne(po => po.Payment)
                   .HasForeignKey<Payment>(p => p.ParentOrderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}