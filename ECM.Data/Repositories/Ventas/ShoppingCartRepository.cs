using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Data.Context;
using ECM.Domain.Entities.Ventas;
using Microsoft.EntityFrameworkCore;

namespace ECM.Data.Repositories.Ventas;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly AppDbContext _context; 

    public ShoppingCartRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // Obtener carrito existente
    public async Task<ShoppingCart?> GetCartWithItemsAsync(int? userId, string? guestId)
    {
        // Validacion 
        if (userId == null && string.IsNullOrWhiteSpace(guestId))
            throw new ArgumentException("Debe proveer un UserId o GuestId válido.");

       
        var result = await _context.Set<ShoppingCart>()
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => 
                (userId != null && c.UserId == userId) || 
                (guestId != null && c.GuestId == guestId));

      
        return result;
    }

    //  Crear nuevo carrito automáticamente
    public async Task<ShoppingCart> AddAsync(ShoppingCart cart)
    {
        // Validacion de nulidad
        if (cart == null)
            throw new ArgumentNullException(nameof(cart), "El carrito no puede ser nulo.");

      
        await _context.Set<ShoppingCart>().AddAsync(cart);
        await _context.SaveChangesAsync();
        
        return cart;
    }

  
}