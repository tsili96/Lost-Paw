using LostPaw.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LostPaw.ModelsConfig
{
    public class AddressEntityTypeConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Country)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Street)
                   .HasMaxLength(100);

            builder.Property(a => a.Number)
                   .HasMaxLength(10);

            builder.Property(a => a.PostalCode)
                   .HasMaxLength(20);

            builder.Property(a => a.Region)
                   .HasMaxLength(100);
        }
    }
}
