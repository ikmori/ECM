using System.Collections.Generic;
using System.Threading.Tasks;
using ECM.Application.Interfaces.BaseRepository;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Application.Interfaces.Respository.Catalogo;


    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> AddAsync(Product entity);
        Task<Product> Update(Product entity, int id);
        Task<Product?> Disable(int id);
        Task<Product?> GetBySkuAsync(string sku);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        
    }