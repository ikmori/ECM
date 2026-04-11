using ECM.Application.Interfaces.BaseRepository;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Logistica;

namespace ECM.Application.Interfaces.Respository.Logistica;

public interface IShipmentRepository : IBaseRepository<Shipment>
{
    Task<Shipment?> GetByTrackingNumberAsync(string trackingNumber);
    Task<Shipment?> GetByOrderIdAsync(int orderId);
    Task<bool> UpdateStatusAsync(int shipmentId, ShipmentStatus newStatus);
    Task<IEnumerable<Shipment>> GetShipmentsByStatusAsync(ShipmentStatus status);
}