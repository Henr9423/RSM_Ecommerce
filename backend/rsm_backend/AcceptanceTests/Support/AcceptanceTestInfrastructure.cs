using Shared.Api;
using Shared.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests.Support
{
    public static class AcceptanceTestInfrastructure
    {
        public static DatabaseFixture Database { get; private set; } = null!;

        public static EcommerceApiFactory ApiFactory { get; private set; } = null!;

        public static async Task StartAsync()
        {
            Database = new DatabaseFixture();
            await Database.StartAsync();

            ApiFactory = new EcommerceApiFactory(Database.ConnectionString);
        }


        public static async Task StopAsync()
        {
            await ApiFactory.DisposeAsync();
            await Database.StopAsync();
        }


    }
}
