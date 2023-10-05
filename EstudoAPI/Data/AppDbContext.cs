using EstudoAPI.Data.Mappings;
using EstudoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EstudoAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new CategoryMap())
            .ApplyConfiguration(new UserMap())
            .ApplyConfiguration(new PostMap())
            .ApplyConfiguration(new RoleMap());
    }
}

