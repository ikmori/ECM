using System.Collections.Generic;
using System.Threading.Tasks;
using ECM.Application.Interfaces.BaseRepository;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Application.Interfaces.Respository.Catalogo;

public interface IProductRepository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task UpdateStockAsync(int productId, int quantity);
    }
}