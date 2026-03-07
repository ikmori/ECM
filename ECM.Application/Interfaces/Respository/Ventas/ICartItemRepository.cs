using ECM.Application.Interfaces.BaseRepository;
using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Interfaces.Respository.Ventas;

public interface ICartItemRepository 
{
    //metodos de busqueda
    Task<CartItem?> GetByIdAsync(int id);
    Task<CartItem?> GetItemInCartAsync(int cartId, int productId);

    // Agregar Producto al Carrito
    Task<CartItem> AddAsync(CartItem entity);

    //  Actualizar Cantidad de Producto
    Task<CartItem> UpdateAsync(CartItem entity);

    // Eliminar Producto Individual
    Task RemoveAsync(CartItem item);
    
    // Elinimar td
    Task RemoveRangeAsync(IEnumerable<CartItem> items);

   
    
}