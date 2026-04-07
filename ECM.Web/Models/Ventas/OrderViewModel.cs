using ECM.Domain.Common.Enums;
namespace ECM.Web.Models.Ventas;

public class OrderViewModel
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public int UserId { get; set; }
        
    public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
}