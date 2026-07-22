using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Data.Configurations.Address
{
    public class AddressConfiguration : IEntityTypeConfiguration<Domain.Entities.Address>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Address> builder)
        {
            builder.ToTable("Addresses");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FullName).IsRequired().HasMaxLength(200);
            builder.Property(a => a.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(a => a.AddressLine1).IsRequired().HasMaxLength(300);
            builder.Property(a => a.AddressLine2).HasMaxLength(300);
            builder.Property(a => a.City).IsRequired().HasMaxLength(100);
            builder.Property(a => a.State).HasMaxLength(100);
            builder.Property(a => a.PostalCode).HasMaxLength(20);
            builder.Property(a => a.IsDefault).IsRequired().HasDefaultValue(false);
            builder.Property(a => a.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");
        }
    }
}