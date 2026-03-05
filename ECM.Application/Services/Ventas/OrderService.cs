using ECM.Application.Interfaces.Respository.Ventas;
using ECM.Application.Interfaces.ServicesInterfaces.OrderService;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Services.Ventas
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _orderRepository.GetOrderWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task CreateAsync(Order entity)
        {
            await _orderRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(Order entity)
        {
            await _orderRepository.Update(entity, entity.Id);
        }

        public async Task DeleteAsync(int id)
        {
            await _orderRepository.Disable(id);
        }

        public async Task<Order> CreateOrderAsync(int userId)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                OrderItems = new List<OrderItem>()
            };

            return await _orderRepository.AddAsync(order);
        }

        public async Task ChangeOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            
            if (order != null)
            {
                order.Status = newStatus;
                await _orderRepository.Update(order, orderId);
            }
        }
    }
}