using ECM.Domain.Common;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Application.ServicesInterfaces.CategoryService;

public interface ICategoryService
{
        Task<OperationResult<IEnumerable<Category>>> GetAllAsync();
        Task<OperationResult<Category>> GetByIdAsync(int id);
        Task<OperationResult<Category>> CreateAsync(Category category);
        Task<OperationResult<Category>> UpdateAsync(Category category);
        Task<OperationResult<bool>> DeleteAsync(int id);
}