using Microsoft.EntityFrameworkCore;
using ECM.Application.Interfaces.Respository.Logistica;
using ECM.Data.Context;
using ECM.Domain.Entities.Logistica;
using ECM.Domain.Common.Enums;

namespace ECM.Data.Repositories.Logistica
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly AppDbContext _context;

        public ShipmentRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Shipment?> GetByIdAsync(int id)
        {
            return await _context.Shipments
                .Include(s => s.Order)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<IEnumerable<Shipment>> GetAllAsync()
        {
            return await _context.Shipments
                .Where(s => s.IsActive)
                .Include(s => s.Order)
                .ToListAsync();
        }

        public async Task<Shipment?> AddAsync(Shipment entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;
            entity.Status = ShipmentStatus.Preparing;
            
            await _context.Shipments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Shipment?> Update(Shipment entity, int id)
        {
            var existingShipment = await _context.Shipments.FindAsync(id);
            if (existingShipment == null) return null;

            entity.Id = id;
            entity.CreatedAt = existingShipment.CreatedAt;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsActive = existingShipment.IsActive;
            
            _context.Entry(existingShipment).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            
            return existingShipment;
        }

        public async Task<Shipment?> Disable(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment != null)
            {
                shipment.IsActive = false;
                shipment.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return shipment;
        }

        // Métodos específicos de IShipmentRepository
        public async Task<Shipment?> GetByTrackingNumberAsync(string trackingNumber)
        {
            return await _context.Shipments
                .Include(s => s.Order)
                .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber && s.IsActive);
        }

        public async Task<Shipment?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Shipments
                .FirstOrDefaultAsync(s => s.OrderId == orderId && s.IsActive);
        }

        public async Task<bool> UpdateStatusAsync(int shipmentId, ShipmentStatus newStatus)
        {
            var shipment = await _context.Shipments.FindAsync(shipmentId);
            if (shipment == null) return false;
            
            shipment.Status = newStatus;
            shipment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Shipment>> GetShipmentsByStatusAsync(ShipmentStatus status)
        {
            return await _context.Shipments
                .Where(s => s.Status == status && s.IsActive)
                .Include(s => s.Order)
                .ToListAsync();
        }
    }
}