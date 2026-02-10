using Domain.Interfaces.Repositories;
using ECM.Domain.Entities.Ventas;


namespace ECM.Application.Interfaces.Respository.Pedidos_y_Ventas;

public interface IOrderRepository
{


    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserAsync(int userId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
    }
}