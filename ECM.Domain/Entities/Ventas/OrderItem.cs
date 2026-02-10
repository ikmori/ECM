using ECM.Domain.Base;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Domain.Entities.Ventas;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
        
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public string ProductName { get; set; } = string.Empty; 
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public OrderItem()
    {
        
    }
}