using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;

public interface IShoppingCartService
{
    //  Obtener o crear el carrito 
    Task<ShoppingCart> GetOrCreateCartAsync(int? userId, string? guestId);
    
    //  Vaciar todos los ítems del carrito
    Task ClearCartAsync(int? userId, string? guestId);
    
}