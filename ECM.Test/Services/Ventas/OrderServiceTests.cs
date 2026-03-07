using System;
using System.Linq;
using System.Threading.Tasks;
using ECM.Application.Services.Ventas;
using ECM.Data.Context;
using ECM.Domain.Common.Enums;
using ECM.Domain.Common;
using ECM.Domain.Entities.Ventas;
using ECM.Data.Repositories.Ventas;
using Microsoft.EntityFrameworkCore;
using Xunit;
namespace ECM.Test.Services.Ventas
{
    public class OrderServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly OrderRepository _repository;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new OrderRepository(_context);
            _service = new OrderService(_repository);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldReturnSuccess()
        {
            // Arrange
            int userId = 10;

            // Act
            var result = await _service.CreateOrderAsync(userId);

            // Assert
            Assert.True(result.Success); 
            Assert.Equal("Orden creada exitosamente.", result.Message); 
            Assert.NotNull(result.Data); 
            Assert.Equal(userId, result.Data.UserId);
            Assert.Equal(OrderStatus.Pending, result.Data.Status);
        }

        [Fact]
        public async Task ChangeOrderStatusAsync_ShouldReturnSuccess()
        {
            // Arrange
           
            var creationResult = await _service.CreateOrderAsync(1);
            int initialOrderId = creationResult.Data.Id;
            
            // Act
            var result = await _service.ChangeOrderStatusAsync(initialOrderId, OrderStatus.Shipped);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Estado de la orden actualizado correctamente.", result.Message);
            
            var updatedOrder = await _context.Orders.FindAsync(initialOrderId);
            Assert.Equal(OrderStatus.Shipped, updatedOrder.Status);
        }

        [Fact]
        public async Task ChangeOrderStatusAsync_ShouldReturnFail()
        {
            // Arrange
            int nonExistentOrderId = 999;
            
            // Act
            var result = await _service.ChangeOrderStatusAsync(nonExistentOrderId, OrderStatus.Cancelled);

            // Assert
            Assert.False(result.Success); 
            Assert.Equal($"No se encontró la orden con el ID {nonExistentOrderId}.", result.Message);
            Assert.Null(result.Data); 
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnSuccessWithAllOrders()
        {
            // Arrange
            await _service.CreateOrderAsync(1);
            await _service.CreateOrderAsync(2);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnSuccess()
        {
            // Arrange
            var creationResult = await _service.CreateOrderAsync(1);
            int orderId = creationResult.Data.Id;

            // Act
            var result = await _service.DeleteAsync(orderId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("La orden fue cancelada exitosamente.", result.Message);

 
            var deletedOrder = await _context.Orders.FindAsync(orderId);
            Assert.Equal(OrderStatus.Cancelled, deletedOrder.Status);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnFail()
        {
            // Arrange
            int nonExistentId = 999;

            // Act
            var result = await _service.GetByIdAsync(nonExistentId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal($"No se encontró la orden con el ID {nonExistentId}.", result.Message);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFail()
        {
            // Arrange
            int nonExistentId = 999;

            // Act
            var result = await _service.DeleteAsync(nonExistentId);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No se puede cancelar la orden porque no existe.", result.Message);
        }
        
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}