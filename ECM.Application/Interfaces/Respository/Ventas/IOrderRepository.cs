//using System.Collections.Generic;
//using System.Threading.Tasks;
using ECM.Application.Interfaces.BaseRepository;
using ECM.Domain.Entities.Ventas;

namespace ECM.Application.Interfaces.Respository.Ventas
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserAsync(int userId);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
    }
}