

using Microsoft.EntityFrameworkCore;
using jwtapp.Entities;
namespace jwtapp.Data;
public class AuthContext : DbContext
{
    public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }
    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasKey(u => u.Id);
        modelBuilder.Entity<UserEntity>().HasIndex(u => u.Username).IsUnique();
    }
}

