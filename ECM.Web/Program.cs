using ECM.Application.Interfaces.Respository.Catalogo;
using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Application.Interfaces.Respository.Logistica;
using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Application.Interfaces.ServicesInterfaces.CouponService;
using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Application.Interfaces.ServicesInterfaces.OrderService;
using ECM.Application.Interfaces.ServicesInterfaces.ProductService;
using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Application.Services.Catalogo;
using ECM.Application.Services.Identidades;
using ECM.Application.Services.Logistica;
using ECM.Application.Services.Ventas;
using ECM.Application.ServicesInterfaces.CategoryService;
using ECM.Data.Context;
using ECM.Data.Repositories.Catalogo;
using ECM.Data.Repositories.Identidades;
using ECM.Data.Repositories.Logistica;
using ECM.Data.Repositories.Ventas;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase(connectionString ?? "ECM_DefaultDb"));

// --- REGISTRO DE REPOSITORIOS ---
// Repositorios de Ventas 
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

// Repositorios de Identidades
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Repositorios de catalogo
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Repositorios de logistica
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();

// --- REGISTRO DE SERVICIOS ---
// Servicios de Ventas
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();

// Servicios de Identidades
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IUserService, UserService>();

// Servicios de catalogo
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Servicios de logistica
builder.Services.AddScoped< CouponService>();
builder.Services.AddScoped< ReviewService>();
builder.Services.AddScoped< ShipmentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
