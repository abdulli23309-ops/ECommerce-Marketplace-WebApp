using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Refunds
{
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            builder.ToTable("Refunds");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(r => r.Status).IsRequired().HasConversion<int>();
            builder.Property(r => r.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            builder.HasOne(r => r.Payment)
                   .WithMany() // no navigation back
                   .HasForeignKey(r => r.PaymentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ReturnRequest)
                   .WithOne() // no navigation back
                   .HasForeignKey<Refund>(r => r.ReturnRequestId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => r.ReturnRequestId).IsUnique(); // one refund per return request
        }
    }
}