using ECM.Application.Interfaces.ServicesInterfaces.Servicio_Base;
using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Interfaces.ServicesInterfaces.OrderService;

public interface IOrderService
{
    public interface IOrderService : IBaseService<Order>
    {
        Task<Order> CreateOrderAsync(int userId);
        Task ChangeOrderStatusAsync(int orderId);
    }
}