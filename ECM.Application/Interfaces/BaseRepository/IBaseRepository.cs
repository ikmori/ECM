using System.Linq.Expressions;

namespace  ECM.Application.Interfaces.BaseRepository
{
    public interface IBaseRepository<T> where T : class
    {
        
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        
        Task<T> AddAsync(T entity);
        Task<T> Update(T entity, int id);
        Task<T> Disable(int id);
    }
}
