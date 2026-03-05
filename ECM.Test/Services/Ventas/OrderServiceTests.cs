
using ECM.Application.Services.Ventas;
using ECM.Data.Context;
using ECM.Domain.Common.Enums;
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
        public async Task CreateOrderAsync_ShouldCreatePendingOrder()
        {
            // Arrange
            int userId = 10;

            // Act
            var result = await _service.CreateOrderAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(OrderStatus.Pending, result.Status);
            Assert.Empty(result.OrderItems);
        }

        [Fact]
        public async Task ChangeOrderStatusAsync_ShouldUpdateStatus_WhenOrderExists()
        {
            // Arrange
            var initialOrder = await _service.CreateOrderAsync(1);
            
            // Act
            await _service.ChangeOrderStatusAsync(initialOrder.Id, OrderStatus.Shipped);

            // Assert
            var updatedOrder = await _context.Orders.FindAsync(initialOrder.Id);
            Assert.Equal(OrderStatus.Shipped, updatedOrder.Status);
        }

        [Fact]
        public async Task ChangeOrderStatusAsync_ShouldNotThrowException_WhenOrderDoesNotExist()
        {
            // Arrange
            int nonExistentOrderId = 999;
            
            // Act
            var exception = await Record.ExceptionAsync(() => 
                _service.ChangeOrderStatusAsync(nonExistentOrderId, OrderStatus.Cancelled));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllOrders()
        {
            // Arrange
            await _service.CreateOrderAsync(1);
            await _service.CreateOrderAsync(2);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task DeleteAsync_ShouldCancelOrder()
        {
            // Arrange
            var order = await _service.CreateOrderAsync(1);

            // Act
            await _service.DeleteAsync(order.Id);

            // Assert
            var deletedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.Equal(OrderStatus.Cancelled, deletedOrder.Status);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}