using ECM.Domain.Base;

namespace ECM.Domain.Entities.Módulo_de_Catálogo;

public class WishlistItem : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}