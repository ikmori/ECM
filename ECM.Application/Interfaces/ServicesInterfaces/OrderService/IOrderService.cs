//using System.Threading.Tasks;
using ECM.Domain.Common.Enums;
using ECM.Application.Interfaces.ServicesInterfaces.Servicio_Base;
using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Interfaces.ServicesInterfaces.OrderService
{
    public interface IOrderService : IBaseService<Order>
    {
        Task<Order> CreateOrderAsync(int userId);
        Task ChangeOrderStatusAsync(int orderId, OrderStatus newStatus);
    }
}