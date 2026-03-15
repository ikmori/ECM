using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Logistica;

namespace ECM.Application.Services.Logistica;

public class ShipmentService
{
    private readonly List<Shipment> _shipments = new();

    public Shipment CreateShipment(int orderId)
    {
        var shipment = new Shipment
        {
            Id = _shipments.Count + 1,
            OrderId = orderId,
            TrackingNumber = GenerateTrackingNumber(),
            CarrierName = "Correos de México",
            Status = ShipmentStatus.Preparing,
            EstimatedDeliveryDate = DateTime.UtcNow.AddDays(5)
        };

        _shipments.Add(shipment);
        return shipment;
    }

    public Shipment? GetShipmentByOrder(int orderId)
    {
        return _shipments.FirstOrDefault(s => s.OrderId == orderId);
    }

    public Shipment? GetShipmentByTracking(string trackingNumber)
    {
        return _shipments.FirstOrDefault(s => s.TrackingNumber == trackingNumber);
    }

    public bool UpdateShipmentStatus(int shipmentId, ShipmentStatus status)
    {
        var shipment = _shipments.FirstOrDefault(s => s.Id == shipmentId);
        if (shipment == null)
            return false;

        shipment.Status = status;
        
        if (status == ShipmentStatus.Delivered)
        {
            shipment.ActualDeliveryDate = DateTime.UtcNow;
        }

        return true;
    }

    private string GenerateTrackingNumber()
    {
        var date = DateTime.UtcNow.ToString("yyMMdd");
        var random = new Random().Next(100000, 999999);
        return $"ECM{date}{random}";
    }
}