using Microsoft.EntityFrameworkCore;
using Npgsql;
using Respawn;
using rsm_backend.Infrastructure.Data;
using Testcontainers.PostgreSql;

namespace Shared.Database;

public class DatabaseFixture
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

    public string ConnectionString => Database.GetConnectionString();

    public async Task StartAsync()
    {
        await Database.StartAsync();

        await using var context = CreateDbContext();

        // Build database using the real application's migrations
        await context.Database.MigrateAsync();

        // Respawn must be configured after the schema exists
        await using var connection =
            new NpgsqlConnection(ConnectionString);

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
            .UseNpgsql(ConnectionString)
            .UseSnakeCaseNamingConvention()
            .Options;

        return new AppDbContext(options);
    }

    public async Task ResetDatabaseAsync()
    {
        await using var connection =
            new NpgsqlConnection(ConnectionString);

        await connection.OpenAsync();

        await _respawner.ResetAsync(connection);
    }

    public async Task StopAsync()
    {
        await Database.DisposeAsync();
    }
}