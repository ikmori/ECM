namespace ECM.Web.Models.Ventas;

public class CartViewModel
{
    public int Id { get; set; }
    public decimal TotalAmount => Items.Sum(i => i.SubTotal);
    public List<CartItemViewModel> Items { get; set; } = new();
    
}