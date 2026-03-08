using System.Collections.Generic;
using System.Threading.Tasks;
using ECM.Domain.Common;

namespace ECM.Application.Interfaces.ServicesInterfaces.Servicio_Base
{
    public interface IBaseService<T> where T : class
    {
        Task<OperationResult<T>> GetByIdAsync(int id);
        Task<OperationResult<IEnumerable<T>>> GetAllAsync();
        Task<OperationResult<T>> CreateAsync(T entity);
        Task<OperationResult<T>> UpdateAsync(T entity);
        Task<OperationResult> DeleteAsync(int id);
    }
}