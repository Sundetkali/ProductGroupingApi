using Microsoft.EntityFrameworkCore;
using ProductGroupingApi.Models;

namespace ProductGroupingApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<ProductGroup>().ToTable("ProductGroups");
        }
    }
}
