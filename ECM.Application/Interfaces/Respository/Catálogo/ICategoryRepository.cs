using Domain.Interfaces.Repositories;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Application.Interfaces.Respository.Catálogo;

public interface ICategoryRepository
{
    
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetWithProductsAsync();
    }
}