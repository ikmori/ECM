using ECM.Domain.Base;
using ECM.Domain.Entities.Módulo_de_Catálogo;

namespace ECM.Domain.Entities.Módulo_de_Logística_y_Marketing;

public class Review : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
        
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int Rating { get; set; } 
    public string? Comment { get; set; }
}