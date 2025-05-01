using Microsoft.EntityFrameworkCore;
using SalesAnalyticsApi.Models;

namespace SalesAnalyticsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = "P123", Name = "UltraBoost Running Shoes", Category = "Shoes" },
                new Product { ProductId = "P456", Name = "iPhone 15 Pro", Category = "Electronics" },
                new Product { ProductId = "P789", Name = "Levi's 501 Jeans", Category = "Clothing" },
                new Product { ProductId = "P234", Name = "Sony WH-1000XM5 Headphones", Category = "Electronics" }
            );

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = "C456", Name = "John Smith", Email = "johnsmith@email.com", Address = "123 Main St, Anytown, CA 12345" },
                new Customer { CustomerId = "C789", Name = "Emily Davis", Email = "emilydavis@email.com", Address = "456 Elm St, Otherville, NY 54321" },
                new Customer { CustomerId = "C101", Name = "Sarah Johnson", Email = "Sarah Johnson", Address = "789 Oak St, New City, TX 75024" }
            );

            //indexes
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.DateOfSale)
                .HasDatabaseName("IX_Orders_DateOfSale");

            modelBuilder.Entity<Order>()
                .HasIndex(o => new { o.DateOfSale, o.Region })
                .HasDatabaseName("IX_Orders_DateOfSale_Region");

            modelBuilder.Entity<Order>()
                .HasIndex(o => new { o.DateOfSale, o.ProductId })
                .HasDatabaseName("IX_Orders_DateOfSale_Product");

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Category)
                .HasDatabaseName("IX_Products_Category");

        }
    }

}
