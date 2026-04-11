namespace ECM.Web.Models.Ventas;

public class CartItemViewModel
{
    public int Id { get; set; } 
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty; 
    public decimal UnitPrice { get; set; } 
    public int Quantity { get; set; }
    public decimal SubTotal => UnitPrice * Quantity;
}