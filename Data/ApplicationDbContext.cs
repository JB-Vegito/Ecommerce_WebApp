using ecommerce_web.Models;
using Microsoft.EntityFrameworkCore;
namespace ecommerce_web.Data;

public class ApplicationDbContext:DbContext{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<Category>()
        .HasAlternateKey(c => c.CategoryName)
        .HasName("AlternateKey_CategoryName");
        
    }
    public DbSet<Category> Categories{get;set;}
    public DbSet<Inventory> Inventories{get; set;}
    public DbSet<Cart> Carts{get; set;}
    public DbSet<Orders> Orders{get; set;}
    public DbSet<OrderDetails> Shipping{get; set;}
}