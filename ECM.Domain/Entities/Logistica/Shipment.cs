using ECM.Domain.Base;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Ventas;

namespace ECM.Domain.Entities.Logistica;

// Gestión de Envíos y Tracking
public class Shipment : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public string TrackingNumber { get; set; } = string.Empty;
    public string CarrierName { get; set; } = string.Empty; 
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Preparing;
    public DateTime? EstimatedDeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }

    public Shipment()
    {
        
    }
}


