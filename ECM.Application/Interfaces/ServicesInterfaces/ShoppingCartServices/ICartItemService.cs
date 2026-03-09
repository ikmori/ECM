using ECM.Domain.Common;
using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;

public interface ICartItemService
{
    // Agrega un producto nuevo o suma la cantidad si ya existe
    Task<OperationResult<CartItem>> AddItemAsync(int? userId, string? guestId, int productId, int quantity);
    
    // actualizar la cantidad
    Task<OperationResult<CartItem>> UpdateQuantityAsync(int? userId, string? guestId, int cartItemId, int newQuantity);
    
    // Elimina un solo registro
    Task<OperationResult> RemoveItemAsync(int? userId, string? guestId, int cartItemId);
    
   
}