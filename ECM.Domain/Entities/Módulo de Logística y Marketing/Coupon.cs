using ECM.Domain.Base;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Módulo_de_Ventas_y_Pedidos;

namespace ECM.Domain.Entities.Módulo_de_Logística_y_Marketing;

public class Coupon : BaseEntity
{
    public string Code { get; set; } = string.Empty; 
    public DiscountType Type { get; set; } 
    public decimal Value { get; set; } 
    public decimal? MinimumOrderAmount { get; set; } 
    public DateTime ExpirationDate { get; set; }
    public int MaxUsage { get; set; } 
    public int TimesUsed { get; set; }
        
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}