using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Reviews
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Comment).HasMaxLength(1000);
            builder.Property(r => r.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");

            // One-to-One: OrderItem → Review
            builder.HasOne(r => r.OrderItem)
                   .WithOne()  // no navigation back to Review in OrderItem (optional, we can add later)
                   .HasForeignKey<Review>(r => r.OrderItemId)
                   .OnDelete(DeleteBehavior.Restrict); // don't delete review if order item is deleted

            // Unique index on OrderItemId (one review per order item)
            builder.HasIndex(r => r.OrderItemId).IsUnique();

            // One-to-Many: Review → ReviewImages
            builder.HasMany(r => r.ReviewImages)
                   .WithOne(ri => ri.Review)
                   .HasForeignKey(ri => ri.ReviewId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Foreign keys to Product and User (for fast lookups)
            builder.HasOne(r => r.Product)
                    .WithMany()
                    .HasForeignKey(r => r.ProductId)
                    .IsRequired(false)                     // make FK optional
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.User)
                   .WithMany()
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}