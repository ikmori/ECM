using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Domain.Common;
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
    
   public async Task<OperationResult<CartItem>> AddItemAsync(int? userId, string? guestId, int productId, int quantity)
    {
        // validaciones
        if (productId <= 0) 
            return OperationResult<CartItem>.Fail("El ID de producto es invalido.");
        if (quantity <= 0)
            return OperationResult<CartItem>.Fail("La cantidad a agregar debe ser mayor a cero.");

        //  Garantizar que exista el  Carrito
        var cartResult = await _shoppingCartService.GetOrCreateCartAsync(userId, guestId);
        
        if (!cartResult.Success) 
            return OperationResult<CartItem>.Fail(cartResult.Message!);

        //  Verificar si el producto ya esta adentro 
        var cart = cartResult.Data!;
        var existingItem = await _cartItemRepository.GetItemInCartAsync(cart.Id, productId);

       
        if (existingItem != null)
        {
            //  Si existe, acumulamos la cantidad
            existingItem.Quantity += quantity;
            var updatedItem = await _cartItemRepository.UpdateAsync(existingItem);
           
            return OperationResult<CartItem>.Ok(updatedItem, "Cantidad acumulada correctamente en el carrito.");
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
            
            var addedItem = await _cartItemRepository.AddAsync(newItem);
            
            return OperationResult<CartItem>.Ok(addedItem, "Producto nuevo agregado al carrito.");
        }
    }

    public async Task<OperationResult<CartItem>> UpdateQuantityAsync(int? userId, string? guestId, int cartItemId, int newQuantity)
    {
        // Validar IDs y cantidades
        if (cartItemId <= 0) 
            return OperationResult<CartItem>.Fail("El identificador del Item debe ser mayor a cero.");
        
        if (newQuantity <= 0) 
            return OperationResult<CartItem>.Fail("La cantidad debe ser mayor a cero.");

        var cartResult = await _shoppingCartService.GetOrCreateCartAsync(userId, guestId);
        if (!cartResult.Success) return OperationResult<CartItem>.Fail(cartResult.Message!);

        var cart = cartResult.Data!;
        
        var itemToUpdate = cart.Items.FirstOrDefault(i => i.Id == cartItemId);

        // validar si existe
        if (itemToUpdate == null)
            return OperationResult<CartItem>.Fail("El articulo no existe o no pertenece a este carrito."); 

        //  modificamos la cantidad
        itemToUpdate.Quantity = newQuantity;

        
        var updatedItem = await _cartItemRepository.UpdateAsync(itemToUpdate);
        return OperationResult<CartItem>.Ok(updatedItem, "Cantidad actualizada con exito.");
    }
    
    

    public async Task<OperationResult> RemoveItemAsync(int? userId, string? guestId, int cartItemId)
    {
        //  Validar identificador
        if (cartItemId <= 0)
            return OperationResult.Fail("El identificador del item debe ser mayor a cero.");

        var cartResult = await _shoppingCartService.GetOrCreateCartAsync(userId, guestId);
        if (!cartResult.Success) return OperationResult.Fail(cartResult.Message!);

        var cart = cartResult.Data!;
        var itemToRemove = cart.Items.FirstOrDefault(i => i.Id == cartItemId);

        
        if (itemToRemove != null)
        {
            await _cartItemRepository.RemoveAsync(itemToRemove);
            return OperationResult.Ok("Producto eliminado exitosamente del carrito.");
        }

        
        return OperationResult.Ok("La operacion finalizo correctamente (el producto no se encontraba en el carrito).");
    }

    
}