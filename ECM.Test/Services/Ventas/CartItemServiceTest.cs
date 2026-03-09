
using ECM.Application.Interfaces.ServicesInterfaces.ShoppingCartServices;
using ECM.Application.Services.Ventas;
using ECM.Data.Context;
using ECM.Data.Repositories.Ventas;
using ECM.Domain.Common;
using ECM.Domain.Entities.Ventas;
using Microsoft.EntityFrameworkCore;

namespace ECM.Test.Services.Ventas;

public class CartItemServiceTest
{
    private readonly AppDbContext _context;
    private readonly ICartItemService _cartItemService;

    public CartItemServiceTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        var shoppingCartRepository = new ShoppingCartRepository(_context);
        var cartItemRepository = new CartItemRepository(_context);
        var shoppingCartService = new ShoppingCartService(shoppingCartRepository, cartItemRepository);

        _cartItemService = new CartItemService(cartItemRepository, shoppingCartService);
    }

    
    [Fact]
    public async Task AddItemAsync_ShouldFail_WhenProductIdIsInvalid()
    {
        // Arrange
        int userId = 1;
        int invalidProductId = 0;

        // Act
        var result = await _cartItemService.AddItemAsync(userId, null, invalidProductId, 1);
        string expectedMessage = "El ID de producto es invalido.";

        // Assert
        Assert.IsType<OperationResult<CartItem>>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.False(result.Success);
    }

    
    [Fact]
    public async Task AddItemAsync_ShouldAddNewItem_WhenItemDoesNotExist()
    {
        // Arrange
        int userId = 1;
        int productId = 99;
        var cart = new ShoppingCart { UserId = userId };
        _context.Set<ShoppingCart>().Add(cart);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cartItemService.AddItemAsync(userId, null, productId, 2);
        string expectedMessage = "Producto nuevo agregado al carrito.";

        // Assert
        Assert.IsType<OperationResult<CartItem>>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.True(result.Success);
    }

    
    [Fact]
    public async Task AddItemAsync_ShouldAccumulateQuantity_WhenItemExists()
    {
        // Arrange
        int userId = 1;
        int productId = 99;
        var cart = new ShoppingCart { UserId = userId };
        cart.Items.Add(new CartItem { ProductId = productId, Quantity = 2 }); 
        _context.Set<ShoppingCart>().Add(cart);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cartItemService.AddItemAsync(userId, null, productId, 3); 
        string expectedMessage = "Cantidad acumulada correctamente en el carrito.";

        // Assert
        Assert.IsType<OperationResult<CartItem>>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.True(result.Success);
        Assert.Equal(5, result.Data!.Quantity); 
    }

   
    [Fact]
    public async Task UpdateQuantityAsync_ShouldFail_WhenCartItemIdIsInvalid()
    {
        // Arrange
        int userId = 1;
        int invalidCartItemId = 0;

        // Act
        var result = await _cartItemService.UpdateQuantityAsync(userId, null, invalidCartItemId, 5);
        string expectedMessage = "El identificador del Item debe ser mayor a cero.";

        // Assert
        Assert.IsType<OperationResult<CartItem>>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.False(result.Success);
    }

    
    [Fact]
    public async Task UpdateQuantityAsync_ShouldFail_WhenItemNotFound()
    {
        // Arrange
        int userId = 1;
        var cart = new ShoppingCart { UserId = userId };
        _context.Set<ShoppingCart>().Add(cart);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cartItemService.UpdateQuantityAsync(userId, null, 999, 5);
        string expectedMessage = "El articulo no existe o no pertenece a este carrito.";

        // Assert
        Assert.IsType<OperationResult<CartItem>>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.False(result.Success);
    }


    [Fact]
    public async Task UpdateQuantityAsync_ShouldUpdateQuantity_WhenItemIsValid()
    {
        // Arrange
        int userId = 1;
        var cart = new ShoppingCart { UserId = userId };
        var item = new CartItem { ProductId = 50, Quantity = 1 }; 
        cart.Items.Add(item);
        _context.Set<ShoppingCart>().Add(cart);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cartItemService.UpdateQuantityAsync(userId, null, item.Id, 10);
        string expectedMessage = "Cantidad actualizada con exito.";

        // Assert
        Assert.IsType<OperationResult<CartItem>>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.True(result.Success);
        Assert.Equal(10, result.Data!.Quantity); 
    }
    

    
    [Fact]
    public async Task RemoveItemAsync_ShouldFail_WhenCartItemIdIsInvalid()
    {
        // Arrange
        int userId = 1;
        int invalidCartItemId = 0;

        // Act
        var result = await _cartItemService.RemoveItemAsync(userId, null, invalidCartItemId);
        string expectedMessage = "El identificador del item debe ser mayor a cero.";

        // Assert
        Assert.IsType<OperationResult>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.False(result.Success);
    }
    
    
    [Fact]
    public async Task RemoveItemAsync_ShouldReturnOk_WhenItemDoesNotExist()
    {
        // Arrange
        int userId = 1;
        var cart = new ShoppingCart { UserId = userId };
        _context.Set<ShoppingCart>().Add(cart);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cartItemService.RemoveItemAsync(userId, null, 999);
        string expectedMessage = "La operacion finalizo correctamente (el producto no se encontraba en el carrito).";

        // Assert
        Assert.IsType<OperationResult>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.True(result.Success);
    }

 
    [Fact]
    public async Task RemoveItemAsync_ShouldRemoveItem_WhenItemExists()
    {
        // Arrange
        int userId = 1;
        var cart = new ShoppingCart { UserId = userId };
        var item = new CartItem { ProductId = 70, Quantity = 1 };
        cart.Items.Add(item);
        _context.Set<ShoppingCart>().Add(cart);
        await _context.SaveChangesAsync();

        // Act
        var result = await _cartItemService.RemoveItemAsync(userId, null, item.Id);
        string expectedMessage = "Producto eliminado exitosamente del carrito.";

        // Assert
        Assert.IsType<OperationResult>(result);
        Assert.Equal(expectedMessage, result.Message);
        Assert.True(result.Success);
        
        var itemExists = await _context.Set<CartItem>().AnyAsync(i => i.Id == item.Id);
        Assert.False(itemExists); 
    }
    
}