using ECM.Domain.Base;
using ECM.Domain.Entities.Catalogo;
using ECM.Domain.Entities.Identidades;

namespace ECM.Domain.Entities.Logistica;

public class Review : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
        
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int Rating { get; set; } 
    public string? Comment { get; set; }

    public Review()
    {
        
    }
}
