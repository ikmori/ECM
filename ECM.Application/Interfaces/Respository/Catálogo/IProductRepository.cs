using Domain.Interfaces.Repositories;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Application.Interfaces.Respository.Catálogo;

public interface IProductRepository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task UpdateStockAsync(int productId, int quantity);
    }
}