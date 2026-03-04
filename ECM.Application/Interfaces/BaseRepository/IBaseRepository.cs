using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ECM.Domain.Entities.Ventas;

namespace  ECM.Application.Interfaces.BaseRepository
{
    public interface IBaseRepository<T> where T : class
    {
        
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        
        Task<T> AddAsync(T entity);
        Task<T> Update(T entity, int id);
        Task<T> Disable(int id);
    }
}
