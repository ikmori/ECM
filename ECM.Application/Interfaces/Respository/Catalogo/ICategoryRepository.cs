using System.Collections.Generic;
using System.Threading.Tasks;
using ECM.Application.Interfaces.BaseRepository;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Application.Interfaces.Respository.Catalogo;

public interface ICategoryRepository
{
    
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> GetWithProductsAsync();
    }
}