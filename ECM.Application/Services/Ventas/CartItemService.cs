using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Services.Ventas;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IShoppingCartService _shoppingCartService;

    public CartItemService(
        ICartItemRepository cartItemRepository, 
        IShoppingCartService shoppingCartService)
    {
        _cartItemRepository = cartItemRepository;
        _shoppingCartService = shoppingCartService;
    }
    
   public async Task<CartItem> AddItemAsync(int? userId, string? guestId, int productId, int quantity)
    {
        if (productId <= 0) throw new ArgumentException("ID de producto inválido.");
        if (quantity <= 0) throw new ArgumentException("La cantidad a agregar debe ser mayor a cero.");

        //  Garantizar que exista el  Carrito
        var cart = await _shoppingCartService.GetOrCreateCartAsync(userId, guestId);

        //  Verificar si el producto ya esta adentro 
        var existingItem = await _cartItemRepository.GetItemInCartAsync(cart.Id, productId);

        if (existingItem != null)
        {
            //  Si existe, acumulamos la cantidad
            existingItem.Quantity += quantity;
            return await _cartItemRepository.UpdateAsync(existingItem);
        }
        else
        {
            //  Si no existe, instanciamos uno nuevo
            var newItem = new CartItem 
            { 
                ShoppingCartId = cart.Id, 
                ProductId = productId, 
                Quantity = quantity 
            };
            return await _cartItemRepository.AddAsync(newItem);
        }
    }

    public async Task<CartItem> UpdateQuantityAsync(int? userId, string? guestId, int cartItemId, int newQuantity)
    {
        if (cartItemId <= 0)
            throw new ArgumentException("El identificador del item es inválido.");

        if (newQuantity <= 0) 
            throw new ArgumentException("La cantidad debe ser mayor a cero.");

        var cart = await _shoppingCartService.GetOrCreateCartAsync(userId, guestId);

        var itemToUpdate = await _cartItemRepository.GetByIdAsync(cartItemId);

        if (itemToUpdate == null || itemToUpdate.ShoppingCartId != cart.Id)
            throw new KeyNotFoundException("El artículo no existe o no pertenece a este carrito.");

        itemToUpdate.Quantity = newQuantity;

        return await _cartItemRepository.UpdateAsync(itemToUpdate);
    }
    
    

    public async Task RemoveItemAsync(int? userId, string? guestId, int cartItemId)
    {
        if (cartItemId <= 0)
            throw new ArgumentException("El identificador del item es inválido.");
        
        //  Validar  para evitar que alguien borre ítems de otro usuario
        var cart = await _shoppingCartService.GetOrCreateCartAsync(userId, guestId);

        var itemToRemove = cart.Items.FirstOrDefault(i => i.Id == cartItemId);

        //  Si existe y es suyo, lo borramos.
        if (itemToRemove != null)
        {
            await _cartItemRepository.RemoveAsync(itemToRemove);
        }
    }

    
}