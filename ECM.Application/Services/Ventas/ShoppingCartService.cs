
using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Domain.Common;
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


    public async Task<OperationResult<ShoppingCart>> GetOrCreateCartAsync(int? userId, string? guestId)
    {
        //validar el usuario
        if ((userId == null || userId <= 0) && string.IsNullOrWhiteSpace(guestId))
            return OperationResult<ShoppingCart>.Fail("Se requiere un identificador de usuario o invitado valido.");

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

        return OperationResult<ShoppingCart>.Ok(cart, "Carrito obtenido exitosamente.");

    }
    
    

    public async Task<OperationResult>ClearCartAsync(int? userId, string? guestId)
    {
        
        // Validacion de usuario
        if ((userId == null || userId <= 0) && string.IsNullOrWhiteSpace(guestId))
            return OperationResult.Fail("Identificador de usuario o invitado invalido.");
        
        var cart = await _shoppingCartRepository.GetCartWithItemsAsync(userId, guestId);

       
       
        if (cart == null || !cart.Items.Any())
            return OperationResult.Ok("El carrito ya se encuentra vacio.");
        
        await _cartItemRepository.RemoveRangeAsync(cart.Items);
            
            return OperationResult.Ok("El carrito se vacio correctamente.");
    }
}
    
    
  