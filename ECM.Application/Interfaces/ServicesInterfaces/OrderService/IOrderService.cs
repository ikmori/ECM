//using System.Threading.Tasks;
using ECM.Domain.Common.Enums;
using ECM.Domain.Common;
using ECM.Application.Interfaces.ServicesInterfaces.Servicio_Base;
using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Interfaces.ServicesInterfaces.OrderService
{
    public interface IOrderService
    {
        Task<OperationResult<Order>> GetByIdAsync(int id);
        Task<OperationResult<IEnumerable<Order>>> GetAllAsync();
        Task<OperationResult<Order>> CreateOrderAsync(int userId);
        Task<OperationResult<Order>> ChangeOrderStatusAsync(int orderId, OrderStatus newStatus);
        Task<OperationResult<bool>> DeleteAsync(int id);
        
        //esto es temporal para probar el modulo de order item en la vista web
        Task<OperationResult<Order>> AddOrderItemAsync(int orderId, int productId, string productName, int quantity, decimal unitPrice);
    }
}