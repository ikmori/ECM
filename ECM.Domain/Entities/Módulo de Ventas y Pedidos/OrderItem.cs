using ECM.Domain.Base;
using ECM.Domain.Entities.Módulo_de_Catálogo;

namespace ECM.Domain.Entities.Módulo_de_Ventas_y_Pedidos;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
        
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string ProductName { get; set; } = string.Empty; 
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } 
}