using FullTractor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FullTractor.Infrastructure.Context;

public class FullTractorContext : DbContext
{
    public FullTractorContext(DbContextOptions<FullTractorContext> dbContextOptions) : base(dbContextOptions) { }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
    }
}