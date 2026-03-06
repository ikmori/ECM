
using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Services.Ventas;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ICartItemRepository _cartItemRepository;

    public ShoppingCartService(
        IShoppingCartRepository shoppingCartRepository,
        ICartItemRepository cartItemRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _cartItemRepository = cartItemRepository;
    }


    public async Task<ShoppingCart> GetOrCreateCartAsync(int? userId, string? guestId)
    {
        
        if (userId == null && string.IsNullOrWhiteSpace(guestId))
            throw new ArgumentException("Se requiere un identificador de usuario o invitado válido.");

        // buscar el carrito existente
        var cart = await _shoppingCartRepository.GetCartWithItemsAsync(userId, guestId);

        //  Si no existe, lo creamos de manera automatica
        if (cart == null)
        {
            cart = new ShoppingCart
            {
                UserId = userId,
                GuestId = guestId
            };

            cart = await _shoppingCartRepository.AddAsync(cart);
        }

        return cart;
    }

    public async Task ClearCartAsync(int? userId, string? guestId)
    {
        
        if (userId == null && string.IsNullOrWhiteSpace(guestId))
            throw new ArgumentException("Identificador inválido.");
        
        var cart = await _shoppingCartRepository.GetCartWithItemsAsync(userId, guestId);

       
        if (cart != null && cart.Items.Any())
        {
            await _cartItemRepository.RemoveRangeAsync(cart.Items);
        }
    }
}
    
    
  