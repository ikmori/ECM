using ECM.Domain.Base;
using ECM.Domain.Entities.Logistica;

namespace ECM.Domain.Entities.Catalogo;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty; // Código único de inventario
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string MainImageUrl { get; set; } = string.Empty;

    // FK
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>(); 
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public Product()
    {
        
    }
}