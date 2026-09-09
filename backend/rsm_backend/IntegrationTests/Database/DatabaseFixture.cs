using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using rsm_backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace IntegrationTests.Database
{
    public class DatabaseFixture:IAsyncLifetime
    {
        public PostgreSqlContainer Database { get; }
        private Respawner _respawner = null!;
        public DatabaseFixture()
        {
            Database = new PostgreSqlBuilder("postgres:16")
                .WithDatabase("ecommerce_test")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();
        }


        public async Task InitializeAsync()
        {
            await Database.StartAsync();

            await using var context = CreateDbContext();

            // Build schema from your real migrations
            await context.Database.MigrateAsync();

            // Configure Respawn after the schema exists
            await using var connection =
                new NpgsqlConnection(Database.GetConnectionString());

            await connection.OpenAsync();

            _respawner = await Respawner.CreateAsync(
                connection,
                new RespawnerOptions
                {
                    DbAdapter = DbAdapter.Postgres,
                    SchemasToInclude = ["public"]
                });
        }

        public AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(Database.GetConnectionString())
                .Options;

            return new AppDbContext(options);
        }


        public async Task ResetDatabaseAsync()
        {
            await using var connection =
                new NpgsqlConnection(Database.GetConnectionString());

            await connection.OpenAsync();

            await _respawner.ResetAsync(connection);
        }

        public async Task DisposeAsync()
        {
            await Database.DisposeAsync();
        }

    }
}
