using LostPaw.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LostPaw.ModelsConfig
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FullName)
                   .HasMaxLength(100);

            builder.Property(u => u.ProfilePicUrl)
                   .HasMaxLength(255);

            builder.Property(u => u.AboutMe)
                   .HasMaxLength(1000);

            builder.Property(u => u.ShowPhoneNumber)
                   .HasDefaultValue(false);

            builder.Property(u => u.ShowFullName)
                   .HasDefaultValue(false);

            builder.HasMany(u => u.Posts)
                   .WithOne(p => p.User)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
