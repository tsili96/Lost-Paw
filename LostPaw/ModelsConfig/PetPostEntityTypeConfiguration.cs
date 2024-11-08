using LostPaw.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LostPaw.ModelsConfig
{
    public class PetPostEntityTypeConfiguration : IEntityTypeConfiguration<PetPost>
    {
        public void Configure(EntityTypeBuilder<PetPost> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User).WithMany(x => x.Posts).HasForeignKey(x => x.UserId);

            //builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
            builder.OwnsOne(x => x.Address);
        }
    }
}
