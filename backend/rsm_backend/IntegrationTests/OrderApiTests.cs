using FluentAssertions;
using IntegrationTests.Api;
using IntegrationTests.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    [Collection("Database")]
    public class OrderApiTests:IAsyncLifetime
    {
        private readonly DatabaseFixture _database;
        private readonly EcommerceApiFactory _factory;
        private readonly HttpClient _client;
        public OrderApiTests(DatabaseFixture database)
        {
            _database = database;
            _factory = new EcommerceApiFactory(
                        database.Database.GetConnectionString());

            _client = _factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            await _database.ResetDatabaseAsync();
            
        }

        public async Task DisposeAsync()
        {
            _client.Dispose();

            await _factory.DisposeAsync();
        }



        [Fact]
        public async Task GetOrder_WhenOrderExists_ShouldReturnOk()
        {
            await using var factory = new EcommerceApiFactory(_database.Database.GetConnectionString());

            var client = factory.CreateClient();

            

            //var response = await client.GetAsync("/api/orders/1234");

            //response.StatusCode
            //    .Should()
            //    .Be(HttpStatusCode.OK);

        }
    }
}
