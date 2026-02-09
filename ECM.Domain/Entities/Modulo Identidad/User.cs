using ECM.Domain.Base;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Módulo_de_Catálogo;
using ECM.Domain.Entities.Módulo_de_Logística_y_Marketing;
using ECM.Domain.Entities.Módulo_de_Ventas_y_Pedidos;

namespace ECM.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    
    // Por defecto
    public UserRole Role { get; set; } = UserRole.Customer; 

    // Para Login Social (Google/Facebook)
    public string? SocialProvider { get; set; } 
    public string? SocialProviderKey { get; set; }

    
    // Relaciones
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<WishlistItem> Wishlist { get; set; } = new List<WishlistItem>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
