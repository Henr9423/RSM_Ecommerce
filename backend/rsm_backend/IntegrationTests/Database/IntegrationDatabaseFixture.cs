using Shared.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Database
{
    public class IntegrationDatabaseFixture:IAsyncLifetime
    {
        public DatabaseFixture Database { get; } = new();

        public Task InitializeAsync()
        {
            return Database.StartAsync();
        }

        public Task DisposeAsync()
        {
            return Database.StopAsync();
        }

      



    }
}
