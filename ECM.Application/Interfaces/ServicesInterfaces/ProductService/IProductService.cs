
using ECM.Domain.Common;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Application.Interfaces.ServicesInterfaces.ProductService;

public interface IProductService
{
    Task<OperationResult<IEnumerable<Product>>> GetAllAsync();
    Task<OperationResult<Product>> GetByIdAsync(int id);
    Task<OperationResult<Product>> CreateAsync(Product product);
    Task<OperationResult<Product>> UpdateAsync(Product product);
    Task<OperationResult<bool>> DeleteAsync(int id);
    Task<OperationResult<bool>> UpdateStockAsync(int productId, int quantity);
}