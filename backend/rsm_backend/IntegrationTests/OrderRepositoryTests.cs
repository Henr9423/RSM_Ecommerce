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
        private readonly DatabaseFixture dbFixture;

        public OrderRepositoryTests(DatabaseFixture fixture)
        {
            dbFixture = fixture;
        }

        public async Task InitializeAsync()
        {
            await dbFixture.ResetDatabaseAsync();
        }
        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }


        [Fact]
        public async Task GetByUserId_WhenOrderExists_ShouldReturnOrder()
        {
            // Arrange
            await using var context = dbFixture.CreateDbContext();

            var repository = new OrderRepository(context);

           
            // create test data...

            // Act
                //var result = await repository.GetByUserId(orderId, userId);

            // Assert
                //result.Should().NotBeNull();

        }

      
    }
}
