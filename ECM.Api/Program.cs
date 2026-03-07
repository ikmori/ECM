using System;
using System.Linq;
using ECM.Application.Interfaces.Respository.Identidades;
using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Application.Interfaces.ServicesInterfaces.IdentidadesServices;
using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Application.Services.Identidades;
using ECM.Application.Services.Ventas;
using ECM.Data;
using ECM.Data.Context;
using ECM.Data.Repositories.Identidades;
using ECM.Data.Repositories.Ventas;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Configurar EF Core para usar la base de datos en
builder.Services.AddDbContext<AppDbContext>(options =>     
    options.UseInMemoryDatabase("EcommerceDb"));

// El nombre "EcommerceDb" identifica la base de datosvar app = builder.Build();

//

//dependecias repositoy y service de shoppingCart y CartItem
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();

//dependencias repository y service de Addresses y Users
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAddressService, IAddressService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

//
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}