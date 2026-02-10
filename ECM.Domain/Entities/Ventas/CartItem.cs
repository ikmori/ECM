using ECM.Domain.Base;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Domain.Entities.Ventas;

public class CartItem : BaseEntity
{
    public int ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }

    public CartItem()
    {
        
    }
}