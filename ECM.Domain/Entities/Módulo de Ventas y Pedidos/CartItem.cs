using ECM.Domain.Base;
using ECM.Domain.Entities.Módulo_de_Catálogo;

namespace ECM.Domain.Entities.Módulo_de_Ventas_y_Pedidos;

public class CartItem : BaseEntity
{
    public int ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}