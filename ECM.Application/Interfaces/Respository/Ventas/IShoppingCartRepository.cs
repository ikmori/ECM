using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Interfaces.Respository.Ventas;

public interface IShoppingCartRepository
{
    
    // Busca el carrito maestro y hace un "Include" de sus CartItems.
    Task<ShoppingCart?> GetCartWithItemsAsync(int? userId, string? guestId);
    
    // Guarda el registro maestro cuando el usuario es nuevo.
    Task<ShoppingCart> AddAsync(ShoppingCart cart);
    
}