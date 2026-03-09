using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Data.Context;
using ECM.Domain.Entities.Ventas;
using Microsoft.EntityFrameworkCore;

namespace ECM.Data.Repositories.Ventas;

public class CartItemRepository :  ICartItemRepository
{
    private readonly AppDbContext _context; 

    public CartItemRepository(AppDbContext context)
    {
        _context = context;
    }

 
    //   Busca un registro específico por su llave primaria. 
   
    public async Task<CartItem?> GetByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentException("El ID provisto no es valido.");
        
        var result = await _context.Set<CartItem>().FindAsync(id);
        
        return result; 
    }

   
    //  Verifica si un producto ya está dentro de un carrito.
    //  para saber si insertamos uno nuevo o sumamos la cantidad.
    
    public async Task<CartItem?> GetItemInCartAsync(int cartId, int productId)
    {
        if (cartId <= 0 || productId <= 0) 
            throw new ArgumentException("Los identificadores deben ser mayores a cero.");

        var result = await _context.Set<CartItem>()
            .FirstOrDefaultAsync(i => i.ShoppingCartId == cartId && i.ProductId == productId);

        return result;
    }

   
  
  //agregar prodcuto al carrito
    public async Task<CartItem> AddAsync(CartItem entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        if (entity.ProductId <= 0) throw new ArgumentException("El producto no es valido.");
        if (entity.Quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");

      
        await _context.Set<CartItem>().AddAsync(entity);
        
      
        await _context.SaveChangesAsync();

        var result = entity;
        return result;
    }
    
  //cambiar cantidad 
    public async Task<CartItem> UpdateAsync(CartItem entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        if (entity.Id <= 0) throw new ArgumentException("El registro debe tener un ID valido para actualizarse.");

       
        _context.Set<CartItem>().Update(entity);
        
       
        await _context.SaveChangesAsync();

        var result = entity;
        return result;
    }

  
  
    //eliminar producto del carrito
    public async Task RemoveAsync(CartItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

      
        _context.Set<CartItem>().Remove(item);
        
     
        await _context.SaveChangesAsync();
    }

   
    //limpiar carrito 
    public async Task RemoveRangeAsync(IEnumerable<CartItem>? items)
    {
        if (items == null) return;

      
        var itemList = items.ToList(); 
        
        if (!itemList.Any()) 
            return; 

        _context.Set<CartItem>().RemoveRange(itemList);
        await _context.SaveChangesAsync();
    }
}
