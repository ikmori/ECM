using ECM.Application.Interfaces.Respository.Catalogo;
using ECM.Application.Services.Catalogo;
using ECM.Application.ServicesInterfaces.CategoryService;
using ECM.Domain.Common;
using ECM.Domain.Entities.Catalogo;
using Moq;
using Xunit;




namespace ECM.Test.Services.catalogo;
public class CategoryServiceTest
{
    private readonly ICategoryService _categoryService;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;

    public CategoryServiceTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryService = new CategoryService(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnSuccess_WithList()
    {
        // Arrange
        var categories = new List<Category> { new Category { Id = 1, Name = "Test" } };
        _categoryRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

        // Act
        var result = await _categoryService.GetAllAsync();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ShouldReturnFail()
    {
        // Arrange
        _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null!);

        // Act
        var result = await _categoryService.GetByIdAsync(1);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Categoría no encontrada.", result.Message);
    }

    [Fact]
    public async Task CreateAsync_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        var category = new Category { Name = "Nueva Cat" };
        _categoryRepositoryMock.Setup(repo => repo.AddAsync(category)).ReturnsAsync(category);

        // Act
        var result = await _categoryService.CreateAsync(category);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Categoría creada.", result.Message);
        _categoryRepositoryMock.Verify(r => r.AddAsync(category), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenIdDoesNotExist_ShouldReturnFail()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Update" };
        _categoryRepositoryMock.Setup(repo => repo.Update(category, category.Id)).ReturnsAsync((Category)null!);

        // Act
        var result = await _categoryService.UpdateAsync(category);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Categoría no encontrada.", result.Message);
    }

    [Fact]
    public async Task DeleteAsync_WhenSuccessful_ShouldReturnTrue()
    {
        // Arrange
        _categoryRepositoryMock.Setup(repo => repo.Disable(1)).ReturnsAsync(new Category { Id = 1 });

        // Act
        var result = await _categoryService.DeleteAsync(1);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Categoría desactivada.", result.Message);
        Assert.True(result.Data);
    }
}