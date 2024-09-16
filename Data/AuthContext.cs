

using Microsoft.EntityFrameworkCore;
using jwtapp.Entities;
namespace jwtapp.Data;
public class AuthContext : DbContext
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<NamesEntity> Names => Set<NamesEntity>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Names)  // Establish the one-to-one or one-to-many relationship
                .WithOne()            // Use `.WithMany()` or `.WithOne()` depending on your relationship
                .HasForeignKey<UserEntity>(u => u.NamesId)
                .OnDelete(DeleteBehavior.Cascade);  // Add the cascade behavior explicitly if necessary

        modelBuilder.Entity<UserEntity>().HasKey(u => u.Id);
        modelBuilder.Entity<UserEntity>().HasIndex(u => u.Username).IsUnique();
    }
}

