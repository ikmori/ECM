using Domain.Interfaces.Repositories;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Application.Interfaces.ServicesInterfaces.ProductService;

public interface IProductService
{
    public interface IProductService : IBaseService<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<bool> DecreaseStockAsync(int productId, int quantity);
    }
}