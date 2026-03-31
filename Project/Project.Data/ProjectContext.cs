using Microsoft.EntityFrameworkCore;
using Project.Data.Models;

namespace Project.Data
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("UserDto").HasKey(u => u.UserId);
            modelBuilder.Entity<Product>().ToTable("ProductDto").HasKey(p => p.ProductId);
            modelBuilder.Entity<Order>().ToTable("OrderDto").HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderDetails>().ToTable("OrderDetailsDto").HasKey(o => o.OrderDetailId);
        }
    }
}
