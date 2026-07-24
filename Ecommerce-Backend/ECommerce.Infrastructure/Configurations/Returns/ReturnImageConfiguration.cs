using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Returns
{
    public class ReturnImageConfiguration : IEntityTypeConfiguration<ReturnImage>
    {
        public void Configure(EntityTypeBuilder<ReturnImage> builder)
        {
            builder.ToTable("ReturnImages");
            builder.HasKey(ri => ri.Id);
            builder.Property(ri => ri.ImageUrl).IsRequired().HasMaxLength(500);
        }
    }
}