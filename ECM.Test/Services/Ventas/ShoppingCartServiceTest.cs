using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Application.Services.Ventas;
using ECM.Data.Context;
using ECM.Data.Repositories.Ventas;
using ECM.Domain.Common;
using ECM.Domain.Entities.Ventas;
using Microsoft.EntityFrameworkCore;

namespace ECM.Test.Services.Ventas;

public class ShoppingCartServiceTest
{
    private readonly AppDbContext _context;
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartServiceTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        var shoppingCartRepository = new ShoppingCartRepository(_context);
        var cartItemRepository = new CartItemRepository(_context);

        _shoppingCartService = new ShoppingCartService(shoppingCartRepository, cartItemRepository);
    }

    
    [Fact]
    public async Task GetOrCreateCartAsync_ShouldFail_WhenIdsAreInvalid()
    {
        // Arrange
        int userId = 0;
        string guestId = "";

        // Act
        var result = await _shoppingCartService.GetOrCreateCartAsync(userId, guestId);
        string expectedMessage = "Se requiere un identificador de usuario o invitado valido.";

        // Assert
        Assert.IsType<OperationResult<ShoppingCart>>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.False(result.Success);
    }
    
   
    [Fact]
    public async Task GetOrCreateCartAsync_ShouldReturnCart_WhenCartExists()
    {
        // Arrange
        int userId = 1;
        var cart = new ShoppingCart { UserId = userId };
        _context.Set<ShoppingCart>().Add(cart);
        await _context.SaveChangesAsync();

        // Act
        var result = await _shoppingCartService.GetOrCreateCartAsync(userId, null);
        string expectedMessage = "Carrito obtenido exitosamente.";

        // Assert
        Assert.IsType<OperationResult<ShoppingCart>>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.True(result.Success);
        Assert.Equal(cart.Id, result.Data!.Id);
    }

    
    
    [Fact]
    public async Task GetOrCreateCartAsync_ShouldCreateCart_WhenCartNotFound()
    {
        // Arrange
        int newUserId = 2;

        // Act
        var result = await _shoppingCartService.GetOrCreateCartAsync(newUserId, null);
        string expectedMessage = "Carrito obtenido exitosamente.";

        // Assert
        Assert.IsType<OperationResult<ShoppingCart>>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
    }
    
    [Fact]
    public async Task GetOrCreateCartAsync_ShouldCreateCart_ForGuest()
    {
        // Arrange
        string guestId = "guest123";

        // Act
        var result = await _shoppingCartService.GetOrCreateCartAsync(null, guestId);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(guestId, result.Data!.GuestId);
    }
    
    
    [Fact]
    public async Task ClearCartAsync_ShouldReturnOk_WhenCartIsEmptyOrNull()
    {
        // Arrange
        int userId = 1;

        // Act
        var result = await _shoppingCartService.ClearCartAsync(userId, null);
        string expectedMessage = "El carrito ya se encuentra vacio.";

        // Assert
        Assert.IsType<OperationResult>(result); 
        Assert.Equal(expectedMessage, result.Message);
        Assert.True(result.Success);
    }
    

    
    
    [Fact]
    public async Task ClearCartAsync_ShouldClearItems_WhenCartHasItems()
    {
        // Arrange
        int userId = 1;
        var cart = new ShoppingCart { UserId = userId };
        cart.Items.Add(new CartItem { ProductId = 100, Quantity = 1 });
        _context.Set<ShoppingCart>().Add(cart);
        await _context.SaveChangesAsync();

        // Act
        var result = await _shoppingCartService.ClearCartAsync(userId, null);
        string expectedMessage = "El carrito se vacio correctamente.";

        // Assert
        Assert.IsType<OperationResult>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.True(result.Success);
        
        var itemsCount = await _context.Set<CartItem>().CountAsync();
        Assert.Equal(0, itemsCount); 
    }
    
    
    [Fact]
    public async Task ClearCartAsync_ShouldReturnOk_WhenCartDoesNotExist()
    {
        // Arrange
        int userId = 50;

        // Act
        var result = await _shoppingCartService.ClearCartAsync(userId, null);

        // Assert
        Assert.True(result.Success);
    }
    
    [Fact]
    public async Task ClearCartAsync_ShouldFail_WhenIdsAreInvalid()
    {
        // Act
        var result = await _shoppingCartService.ClearCartAsync(0, "");

        // Assert
        Assert.False(result.Success);
    }
    
    
    
}