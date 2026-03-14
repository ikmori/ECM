
using ECM.Application.Interfaces.Respository.Catalogo;
using ECM.Application.Interfaces.ServicesInterfaces.ProductService;
using ECM.Domain.Common;
using ECM.Domain.Entities.Catalogo;

namespace ECM.Application.Services.Catalogo;
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<IEnumerable<Product>>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return OperationResult<IEnumerable<Product>>.Ok(products, "Listado de productos recuperado.");
    }

    public async Task<OperationResult<Product>> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
            return OperationResult<Product>.Fail($"El producto con ID {id} no existe.");

        return OperationResult<Product>.Ok(product, "Producto encontrado.");
    }

    public async Task<OperationResult<Product>> CreateAsync(Product product)
    {
        var existingSku = await _repository.GetBySkuAsync(product.SKU);
        if (existingSku != null)
            return OperationResult<Product>.Fail($"Ya existe un producto con el SKU: {product.SKU}.");

        // El repositorio ya hace el SaveChangesAsync y devuelve la entidad
        var result = await _repository.AddAsync(product);
        
        return result != null 
            ? OperationResult<Product>.Ok(result, "Producto registrado correctamente.")
            : OperationResult<Product>.Fail("No se pudo crear el producto.");
    }

    public async Task<OperationResult<Product>> UpdateAsync(Product product)
    {
        // Pasamos la entidad y el ID por separado como pide tu nuevo repositorio
        var result = await _repository.Update(product, product.Id);

        if (result == null)
            return OperationResult<Product>.Fail("Producto no encontrado para actualización.");

        return OperationResult<Product>.Ok(result, "Producto actualizado con éxito.");
    }

    public async Task<OperationResult<bool>> UpdateStockAsync(int productId, int quantity)
    {
        var product = await _repository.GetByIdAsync(productId);
        if (product == null)
            return OperationResult<bool>.Fail("Producto no encontrado.");

        product.StockQuantity += quantity;
        
        if (product.StockQuantity < 0)
            return OperationResult<bool>.Fail("La operación resultaría en un stock negativo.");

        await _repository.Update(product, productId);
        return OperationResult<bool>.Ok(true, "Stock actualizado.");
    }

    public async Task<OperationResult<bool>> DeleteAsync(int id)
    {
        // Cambiamos Delete por Disable para coincidir con el repositorio
        var result = await _repository.Disable(id);
        
        if (result == null)
            return OperationResult<bool>.Fail("No se encontró el producto para desactivar.");

        return OperationResult<bool>.Ok(true, "Producto desactivado exitosamente.");
    }
}