using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Reviews
{
    public class ReviewImageConfiguration : IEntityTypeConfiguration<ReviewImage>
    {
        public void Configure(EntityTypeBuilder<ReviewImage> builder)
        {
            builder.ToTable("ReviewImages");
            builder.HasKey(ri => ri.Id);

            builder.Property(ri => ri.ImageUrl).IsRequired().HasMaxLength(500);
        }
    }
}