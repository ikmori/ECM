using ECM.Domain.Entities.Catalogo;
using ECM.Domain.Entities.Identidades;
using ECM.Domain.Entities.Logistica;
using ECM.Domain.Entities.Ventas;
using Microsoft.EntityFrameworkCore;


namespace ECM.Data;

public class AppDbContext: DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    //indentidad
    public DbSet<User>Users { get; set; }
    public DbSet<Address>Adresses { get; set; }
    
    //catalogo
    public DbSet<Category>Categories { get; set; }
    public DbSet<Product>Products { get; set; }
    public DbSet<ProductImage>ProductImages { get; set; }
    public DbSet<WishlistItem>WishlistItems { get; set; }
    // logistca 
    public DbSet<Coupon>Coupons { get; set; }
    public DbSet<Review>Reviews { get; set; }
    public DbSet<Shipment>Shipments { get; set; }
    // ventas 
    public DbSet<CartItem>CartItems { get; set; }
    public DbSet<Order>Orders { get; set; }
    public DbSet<OrderItem>OrderItems { get; set; }
    public DbSet<ShoppingCart>ShoppingCarts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //modelBuilder.Entity<User>();
    }
    
    
}