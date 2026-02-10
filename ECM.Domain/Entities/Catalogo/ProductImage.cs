using ECM.Domain.Base;

namespace ECM.Domain.Entities.Catalogo;

public class ProductImage : BaseEntity
{
    public string ImageUrl { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public ProductImage()
    {
        
    }
}

