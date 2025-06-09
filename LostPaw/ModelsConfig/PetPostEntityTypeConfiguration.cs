using LostPaw.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LostPaw.ModelsConfig
{
    public class PetPostEntityTypeConfiguration : IEntityTypeConfiguration<PetPost>
    {
        public void Configure(EntityTypeBuilder<PetPost> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Type)
                   .IsRequired();

            builder.Property(p => p.Title)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(p => p.Description)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(p => p.ImageUrl)
                   .HasMaxLength(255);

            builder.Property(p => p.ChipNumber)
                   .HasMaxLength(50);

            builder.Property(p => p.DateCreated)
                   .IsRequired();

            builder.HasOne(p => p.User)
                   .WithMany(u => u.Posts)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Address)
                   .WithMany()
                   .HasForeignKey(p => p.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
