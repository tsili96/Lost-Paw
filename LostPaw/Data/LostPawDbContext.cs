using LostPaw.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LostPaw.Data;

public class LostPawDbContext : IdentityDbContext<User>
{
    public LostPawDbContext(DbContextOptions<LostPawDbContext> options)
        : base(options)
    {
    }
    public DbSet<PetPost> Posts { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Disable cascading deletes for User1 and User2
        builder.Entity<Chat>()
               .HasOne(c => c.User1)
               .WithMany()
               .HasForeignKey(c => c.User1Id)
               .OnDelete(DeleteBehavior.NoAction);  // Prevents cascade delete

        builder.Entity<Chat>()
               .HasOne(c => c.User2)
               .WithMany()
               .HasForeignKey(c => c.User2Id)
               .OnDelete(DeleteBehavior.NoAction);  
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
