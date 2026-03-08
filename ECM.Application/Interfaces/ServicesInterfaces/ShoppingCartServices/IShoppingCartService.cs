using ECM.Domain.Common;
using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;

public interface IShoppingCartService
{
    //  Obtener o crear el carrito 
    Task<OperationResult<ShoppingCart>>GetOrCreateCartAsync(int? userId, string? guestId);
    
    //  Vaciar todos los ítems del carrito
    Task<OperationResult> ClearCartAsync(int? userId, string? guestId);
    
}