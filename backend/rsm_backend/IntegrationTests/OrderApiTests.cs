using FluentAssertions;
using Shared;
using rsm_backend.Domain.Entities;
using rsm_backend.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Shared.Database;
using IntegrationTests.Database;
using Shared.Api;

namespace IntegrationTests
{
    [Collection("Database")]
    public class OrderApiTests:IAsyncLifetime
    {
        private readonly IntegrationDatabaseFixture _dbFixture;
        private readonly EcommerceApiFactory _factory;
        private readonly HttpClient _client;
        public OrderApiTests(IntegrationDatabaseFixture database)
        {
            _dbFixture = database;
            _factory = new EcommerceApiFactory(
                        database.Database.ConnectionString);

            _client = _factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            await _dbFixture.Database.ResetDatabaseAsync();
            
        }

        public async Task DisposeAsync()
        {
            _client.Dispose();

            await _factory.DisposeAsync();
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
                Brand = new Brand() { Name = "test_Brand", CreatedAt = DateTime.UtcNow }

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
                OrderNumber = "123",
                CreatedAt = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                EstimatedDeliveryFrom = new DateTime(2026, 8, 5, 0, 0, 0, DateTimeKind.Utc),
                EstimatedDeliveryTo = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                CouponDiscount = 0m,
                ShippingFee = 0m,
                Subtotal = 199.98m,
                Tax = 0m,

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
        public async Task GetOrders_WhenAuthenticated_ReturnsOk()
        {
            // Pretend this user is logged in
            _client.DefaultRequestHeaders.Add(
                "X-Test-UserId",
                "test-user-123");

            var response =
                await _client.GetAsync("/api/Order");

            response.StatusCode.Should()
                .Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetGuestOrder_WithValidToken_ReturnsOk()
        {
            // Arrange 

            await using var context = _dbFixture.Database.CreateDbContext();

            var repo = new OrderRepository(context);


            Order expectedOrder = CreateValidOrder();
        
            await repo.AddOrderAsync(expectedOrder);


            var token = TestJwtTokenFactory.CreateGuestToken(
                [new Claim("orderNumber", $"{expectedOrder.OrderNumber}"),
                 new Claim("purpose","guest-order-access")]);



            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);



            // Act

            var response =
                await _client.GetAsync($"/api/Order/guest/{expectedOrder.OrderNumber}");

            // Assert

            response.StatusCode.Should()
                .Be(HttpStatusCode.OK,$"response body was: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
