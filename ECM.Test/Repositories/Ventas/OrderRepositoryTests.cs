using System;
using System.Linq;
using System.Threading.Tasks;
using ECM.Data.Context;
using ECM.Domain.Common.Enums;
using ECM.Domain.Entities.Ventas;
using ECM.Data.Repositories.Ventas;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECM.Test.Repositories.Ventas
{
    public class OrderRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new OrderRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveOrderToDatabase()
        {
            // Arrange
            var order = new Order { UserId = 1, Status = OrderStatus.Pending };

            // Act
            var result = await _repository.AddAsync(order);

            // Assert
            Assert.NotEqual(0, result.Id);
            Assert.Equal(1, _context.Orders.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnOrder_WhenExists()
        {
            // Arrange
            var order = new Order { UserId = 2 };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(order.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.UserId);
        }

        [Fact]
        public async Task GetByUserAsync_ShouldReturnOnlyUserOrders()
        {
            // Arrange
            _context.Orders.AddRange(
                new Order { UserId = 1 },
                new Order { UserId = 1 },
                new Order { UserId = 2 }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUserAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, o => Assert.Equal(1, o.UserId));
        }

        [Fact]
        public async Task Update_ShouldModifyExistingOrder()
        {
            // Arrange
            var order = new Order { UserId = 1, Status = OrderStatus.Pending };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            order.Status = OrderStatus.Completed;

            // Act
            await _repository.Update(order, order.Id);

            // Assert
            var updatedOrder = await _context.Orders.FindAsync(order.Id);
            Assert.Equal(OrderStatus.Completed, updatedOrder.Status);
        }

        [Fact]
        public async Task Disable_ShouldChangeStatusToCancelled()
        {
            // Arrange
            var order = new Order { UserId = 1, Status = OrderStatus.Pending };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await _repository.Disable(order.Id);

            // Assert
            var disabledOrder = await _context.Orders.FindAsync(order.Id);
            Assert.Equal(OrderStatus.Cancelled, disabledOrder.Status);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}