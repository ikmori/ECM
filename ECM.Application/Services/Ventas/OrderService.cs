using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECM.Domain.Common;
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

        public async Task<OperationResult<Order>> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return OperationResult<Order>.Fail($"No se encontró la orden con el ID {id}.");

            return OperationResult<Order>.Ok(order, "Orden recuperada con éxito.");
        }

        public async Task<OperationResult<IEnumerable<Order>>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return OperationResult<IEnumerable<Order>>.Ok(orders, "Listado de órdenes recuperado.");
        }

        public async Task<OperationResult<Order>> CreateOrderAsync(int userId)
        {
            if (userId <= 0)
                return OperationResult<Order>.Fail("El ID de usuario no es válido.");

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            await _orderRepository.AddAsync(order);

            return OperationResult<Order>.Ok(order, "Orden creada exitosamente.");
        }

        public async Task<OperationResult<Order>> ChangeOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            
            if (order == null)
                return OperationResult<Order>.Fail($"No se encontró la orden con el ID {orderId}.");

            order.Status = newStatus;
            await _orderRepository.Update(order, orderId);

            return OperationResult<Order>.Ok(order, "Estado de la orden actualizado correctamente.");
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            var order = await _orderRepository.Disable(id);
            
            if (order == null)
                return OperationResult<bool>.Fail("No se puede cancelar la orden porque no existe.");

            return OperationResult<bool>.Ok(true, "La orden fue cancelada exitosamente.");
        }
        
        //esto es temporal para probar el modulo de order item en la vista web
        public async Task<OperationResult<Order>> AddOrderItemAsync(int orderId, int productId, string productName, int quantity, decimal unitPrice)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return OperationResult<Order>.Fail($"No se encontró la orden con el ID {orderId}.");
            
            var newItem = new OrderItem
            {
                OrderId = orderId,
                ProductId = productId,
                ProductName = productName,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
            
            order.OrderItems.Add(newItem);
            
            order.SubTotal += (quantity * unitPrice);
            order.TotalAmount = order.SubTotal + order.ShippingCost - order.DiscountAmount;
            
            await _orderRepository.Update(order, orderId);

            return OperationResult<Order>.Ok(order, $"Producto '{productName}' agregado correctamente.");
        }
    }
}