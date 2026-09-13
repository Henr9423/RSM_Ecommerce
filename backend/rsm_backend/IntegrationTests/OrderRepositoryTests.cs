using FluentAssertions;
using IntegrationTests.Database;
using Microsoft.EntityFrameworkCore;
using rsm_backend.Domain.Entities;
using rsm_backend.Infrastructure.Data;
using rsm_backend.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IntegrationTests
{
    [Collection("Database")]
    public class OrderRepositoryTests:IAsyncLifetime
    {
        private readonly IntegrationDatabaseFixture dbFixture;

        public OrderRepositoryTests(IntegrationDatabaseFixture fixture)
        {
            dbFixture = fixture;
        }

        public async Task InitializeAsync()
        {
            await dbFixture.Database.ResetDatabaseAsync();
        }
        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        private Order CreateValidOrder()
        {
            var image = new ProductImage
            {
                StorageKey = "images/test.jpg"
            };

            var product = new Product
            {
                Name = "Test Product",
                RatingCount = 10,
                AverageRating = 4.5m,
                ProductTags = new List<ProductTag>(),
                Brand=new Brand() { Name="test_Brand", CreatedAt=DateTime.UtcNow }
                
            };

            var variant = new ProductVariant
            {
                Id = 5,
                ProductId = 1,
                Price = 99.99m,
                Product = product,
                ProductImages = new List<ProductImage>
                {
                    image
                }
            };

            return new Order
            {
                Id = 123,
                Total = 199.98m,
                OrderNumber = "Test_OrderNumber",
                CreatedAt = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                EstimatedDeliveryFrom = new DateTime(2026, 8, 5, 0, 0, 0, DateTimeKind.Utc),
                EstimatedDeliveryTo = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                CouponDiscount = 0m,
                ShippingFee = 0m,
                Subtotal = 199.98m,
                Tax=0m,

                Customer = new Customer
                {
                    Email = "test@test.com"
                },

                OrderItems = new List<OrderItem>
                {
                    new OrderItem {
                        Quantity = 2,
                        ProductVariant = variant
                    }
                }
            };
        }

    
        [Fact]
        public async Task GetAllByUserIdAsync_WhenUserIdMatch_ShouldReturnOrders()
        {
            // Arrange
            await using var context = dbFixture.Database.CreateDbContext();

            var repo = new OrderRepository(context);


            Order expectedOrder = CreateValidOrder();
            expectedOrder.Customer = new Customer() { UserId = "test_userId", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, User = new ApplicationUser() { Id = "test_userId" } };

            await repo.AddOrderAsync(expectedOrder);

            // Act
            var result = await repo.GetAllByUserIdAsync(expectedOrder.Customer.UserId);

            // Assert
            result.Should().NotBeNull();
            result.Should().Contain(expectedOrder);

        }

        [Fact]
        public async Task GetByUserIdAsync_WhenUserIdAndOrderIdMatch_ShouldReturnOrder()
        {
            // Arrange
            await using var context = dbFixture.Database.CreateDbContext();

            var repo = new OrderRepository(context);


            Order expectedOrder = CreateValidOrder();
            expectedOrder.Customer = new Customer() { UserId = "test_userId", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, User = new ApplicationUser() { Id = "test_userId" } };

            await repo.AddOrderAsync(expectedOrder);

            // Act
            var result = await repo.GetByUserId(expectedOrder.Id,expectedOrder.Customer.UserId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedOrder);

        }

        [Fact]
        public async Task GetByOrderNumber_WhenOrderExists_ShouldReturnOrder()
        {
            // Arrange
            await using var context = dbFixture.Database.CreateDbContext();

            var repo = new OrderRepository(context);


            var expectedOrder = CreateValidOrder();

            await repo.AddOrderAsync(expectedOrder);

            // Act
            var result = await repo.GetByOrderNumber(expectedOrder.OrderNumber);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedOrder);

        }



    }
}
