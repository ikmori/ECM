using System.Collections.Generic;
using System.Threading.Tasks;
using ECM.Application.Interfaces.BaseRepository;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Application.Interfaces.Respository.Catalogo;

public interface ICategoryRepository
{
    
 
    
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> AddAsync(Category entity);
        Task<Category?> Update(Category entity, int id);
        Task<Category?> Disable(int id);
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);

       
}