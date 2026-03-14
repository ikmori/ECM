using Moq;
using Xunit;
using ECM.Application.Services.Catalogo;
using ECM.Application.Interfaces.Respository.Catalogo;
using ECM.Application.Interfaces.ServicesInterfaces.ProductService;
using ECM.Domain.Entities.Catalogo;
using ECM.Domain.Common;


namespace ECM.Test.Services.catalogo;

public class ProductServiceTest
{
    private readonly IProductService _productService;
    private readonly Mock<IProductRepository> _productRepositoryMock;

    public ProductServiceTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ShouldReturnFailWithIdMessage()
    {
        // Arrange
        int id = 50;
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Product)null!);

        // Act
        var result = await _productService.GetByIdAsync(id);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"El producto con ID {id} no existe.", result.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenSkuExists_ShouldReturnFail()
    {
        // Arrange
        var product = new Product { SKU = "ABC-123", Name = "Producto X" };
        _productRepositoryMock.Setup(repo => repo.GetBySkuAsync(product.SKU)).ReturnsAsync(new Product());

        // Act
        var result = await _productService.CreateAsync(product);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"Ya existe un producto con el SKU: {product.SKU}.", result.Message);
        _productRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotFound_ShouldReturnFail()
    {
        // Arrange
        var product = new Product { Id = 1, SKU = "SKU-1" };
        _productRepositoryMock.Setup(repo => repo.Update(product, product.Id)).ReturnsAsync((Product)null!);

        // Act
        var result = await _productService.UpdateAsync(product);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Producto no encontrado para actualización.", result.Message);
    }

    [Fact]
    public async Task UpdateStockAsync_WhenResultIsNegative_ShouldReturnFail()
    {
        // Arrange
        int id = 1;
        var product = new Product { Id = id, StockQuantity = 5 };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(product);

        // Act
        var result = await _productService.UpdateStockAsync(id, -10); // Quedaría en -5

        // Assert
        Assert.False(result.Success);
        Assert.Equal("La operación resultaría en un stock negativo.", result.Message);
    }

    [Fact]
    public async Task DeleteAsync_WhenSuccessful_ShouldReturnTrue()
    {
        // Arrange
        int id = 10;
        _productRepositoryMock.Setup(repo => repo.Disable(id)).ReturnsAsync(new Product { Id = id });

        // Act
        var result = await _productService.DeleteAsync(id);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Producto desactivado exitosamente.", result.Message);
        Assert.True(result.Data);
    }
}