using ECM.Domain.Base;

namespace ECM.Domain.Entities.Ventas;

// Carrito de Compras Persistente
public class ShoppingCart : BaseEntity
{
    public int? UserId { get; set; } // Nullable para invitados
    public string? GuestId { get; set; } // Cookie ID para no registrados
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    public ShoppingCart()
    {
        
    }
}