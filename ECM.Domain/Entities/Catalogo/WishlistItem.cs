using ECM.Domain.Base;
using ECM.Domain.Entities.Identidades;

namespace ECM.Domain.Entities.Catalogo;

public class WishlistItem : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public WishlistItem()
    {
        
    }
}