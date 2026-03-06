
using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Data.Context;
using ECM.Domain.Entities.Ventas;
using Microsoft.EntityFrameworkCore;

namespace ECM.Data.Repositories.Ventas
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> AddAsync(Order entity)
        {
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Order> Update(Order entity, int id)
        {
            var existingOrder = await _context.Orders.FindAsync(id);
            if (existingOrder == null) return null;

            _context.Entry(existingOrder).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            
            return existingOrder;
        }

        public async Task<Order> Disable(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = ECM.Domain.Common.Enums.OrderStatus.Cancelled;
                await _context.SaveChangesAsync();
            }
            return order;
        }

        public async Task<IEnumerable<Order>> GetByUserAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}