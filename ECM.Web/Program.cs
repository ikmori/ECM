using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Application.Services.Identidades;
using ECM.Application.Services.Ventas;
using ECM.Data.Context;
using ECM.Data.Repositories.Identidades;
using ECM.Data.Repositories.Ventas;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Leemos el nombre desde el appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registramos el DbContext usando ese nombre
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase(connectionString));

// Dependencias repository y service de ShoppingCart y CartItem
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();

// Dependencias repository y service de Addresses y Users
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAddressService, AddressService>(); 
builder.Services.AddScoped<IUserService, UserService>();

// BUILD SOLO UNA VEZ
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