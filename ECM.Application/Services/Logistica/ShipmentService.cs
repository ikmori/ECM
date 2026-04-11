// ==========================================
// ECM.Application.Services.Logistica.ShipmentService.cs
// ==========================================
using ECM.Application.Interfaces.Respository.Logistica;
using ECM.Domain.Common;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Logistica;

namespace ECM.Application.Services.Logistica;

public class ShipmentService
{
    private readonly IShipmentRepository _shipmentRepository;

    public ShipmentService(IShipmentRepository shipmentRepository)
    {
        _shipmentRepository = shipmentRepository;
    }

    public async Task<OperationResult<IEnumerable<Shipment>>> GetAllShipmentsAsync()
    {
        try
        {
            var shipments = await _shipmentRepository.GetAllAsync();
            return OperationResult<IEnumerable<Shipment>>.Ok(shipments, "Envíos recuperados exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<IEnumerable<Shipment>>.Fail($"Error al recuperar envíos: {ex.Message}");
        }
    }

    public async Task<OperationResult<Shipment>> GetShipmentByIdAsync(int id)
    {
        try
        {
            var shipment = await _shipmentRepository.GetByIdAsync(id);
            if (shipment == null)
                return OperationResult<Shipment>.Fail($"Envío con ID {id} no encontrado.");
            
            return OperationResult<Shipment>.Ok(shipment, "Envío recuperado exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<Shipment>.Fail($"Error al recuperar envío: {ex.Message}");
        }
    }

    public async Task<OperationResult<Shipment>> GetShipmentByTrackingNumberAsync(string trackingNumber)
    {
        try
        {
            var shipment = await _shipmentRepository.GetByTrackingNumberAsync(trackingNumber);
            if (shipment == null)
                return OperationResult<Shipment>.Fail($"Envío con tracking {trackingNumber} no encontrado.");
            
            return OperationResult<Shipment>.Ok(shipment, "Envío recuperado exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<Shipment>.Fail($"Error al recuperar envío: {ex.Message}");
        }
    }

    public async Task<OperationResult<Shipment>> GetShipmentByOrderIdAsync(int orderId)
    {
        try
        {
            var shipment = await _shipmentRepository.GetByOrderIdAsync(orderId);
            if (shipment == null)
                return OperationResult<Shipment>.Fail($"No hay envío para la orden {orderId}.");
            
            return OperationResult<Shipment>.Ok(shipment, "Envío recuperado exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<Shipment>.Fail($"Error al recuperar envío: {ex.Message}");
        }
    }

    public async Task<OperationResult<IEnumerable<Shipment>>> GetShipmentsByStatusAsync(ShipmentStatus status)
    {
        try
        {
            var shipments = await _shipmentRepository.GetShipmentsByStatusAsync(status);
            return OperationResult<IEnumerable<Shipment>>.Ok(shipments, $"Envíos con estado {status} recuperados.");
        }
        catch (Exception ex)
        {
            return OperationResult<IEnumerable<Shipment>>.Fail($"Error al recuperar envíos: {ex.Message}");
        }
    }

    public async Task<OperationResult<Shipment>> CreateShipmentAsync(int orderId, string trackingNumber, string carrierName, DateTime? estimatedDeliveryDate)
    {
        try
        {
            // Verificar si ya existe un envío para esta orden
            var existing = await _shipmentRepository.GetByOrderIdAsync(orderId);
            if (existing != null)
                return OperationResult<Shipment>.Fail($"La orden {orderId} ya tiene un envío asignado.");

            var shipment = new Shipment
            {
                OrderId = orderId,
                TrackingNumber = trackingNumber,
                CarrierName = carrierName,
                Status = ShipmentStatus.Preparing,
                EstimatedDeliveryDate = estimatedDeliveryDate,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var created = await _shipmentRepository.AddAsync(shipment);
            if (created == null)
                return OperationResult<Shipment>.Fail("Error al crear el envío.");
            
            return OperationResult<Shipment>.Ok(created, "Envío creado exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<Shipment>.Fail($"Error al crear envío: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> UpdateShipmentStatusAsync(int shipmentId, ShipmentStatus newStatus)
    {
        try
        {
            var result = await _shipmentRepository.UpdateStatusAsync(shipmentId, newStatus);
            if (!result)
                return OperationResult<bool>.Fail($"Envío con ID {shipmentId} no encontrado.");
            
            return OperationResult<bool>.Ok(true, $"Estado actualizado a {newStatus}.");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Fail($"Error al actualizar estado: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> MarkAsDeliveredAsync(int shipmentId)
    {
        try
        {
            var result = await _shipmentRepository.UpdateStatusAsync(shipmentId, ShipmentStatus.Delivered);
            if (!result)
                return OperationResult<bool>.Fail($"Envío con ID {shipmentId} no encontrado.");
            
            return OperationResult<bool>.Ok(true, "Envío marcado como entregado.");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Fail($"Error al marcar como entregado: {ex.Message}");
        }
    }

    public async Task<OperationResult<bool>> CancelShipmentAsync(int shipmentId)
    {
        try
        {
            var shipment = await _shipmentRepository.Disable(shipmentId);
            if (shipment == null)
                return OperationResult<bool>.Fail($"Envío con ID {shipmentId} no encontrado.");
            
            return OperationResult<bool>.Ok(true, "Envío cancelado exitosamente.");
        }
        catch (Exception ex)
        {
            return OperationResult<bool>.Fail($"Error al cancelar envío: {ex.Message}");
        }
    }
}