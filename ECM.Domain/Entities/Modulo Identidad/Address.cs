using ECM.Domain.Base;

namespace ECM.Domain.Entities;

public class Address : BaseEntity
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
        
    // FK
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}