using ECM.Domain.Base;

namespace ECM.Domain.Entities.Módulo_de_Catálogo;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
        
    // Subcategorías (Recursividad)
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
        
    public ICollection<Product> Products { get; set; } = new List<Product>();
}