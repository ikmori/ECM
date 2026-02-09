using ECM.Domain.Base;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Módulo_de_Logística_y_Marketing;

namespace ECM.Domain.Entities.Módulo_de_Ventas_y_Pedidos;

public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal SubTotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; } 
        
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? PaymentTransactionId { get; set; } 

    // Datos de Envío Snapshot (Se guardan aquí por si el usuario cambia su dirección después)
    public string ShippingAddress { get; set; } = string.Empty; 
        
    // FKs
    public int UserId { get; set; }
    public User User { get; set; } = null!;
        
    public int? CouponId { get; set; } // Si usó cupón
    public Coupon? Coupon { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public Shipment? Shipment { get; set; } 
}