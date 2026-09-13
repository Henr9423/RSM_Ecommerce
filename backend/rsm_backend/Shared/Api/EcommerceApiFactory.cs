
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using rsm_backend.Application.Services.Interfaces.Infrastructure;
using rsm_backend.Infrastructure.Data;
using rsm_backend.Infrastructure.FakeServicesForTests;


namespace Shared.Api
{
    public class EcommerceApiFactory : WebApplicationFactory<rsm_backend.Api.Program>
    {
        private readonly string _connectionString;
        public FakeObjectStorage _objectStorage { get; } = new();
        public FakeEmailService _emailService { get; } = new();

  
        public const string TestJwtKey =
        "integration-test-secret-key-that-is-long-enough-123";

        public EcommerceApiFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting(
                "GuestJwt:Secret",
                TestJwtKey);

            builder.UseSetting(
                "GuestJwt:Issuer",
                "rsm-backend");

            builder.UseSetting(
                "GuestJwt:Audience",
                "rsm-guest-orders");

            builder.UseSetting(
                "GuestJwt:ExpirationMinutes",
                "30");

            builder.UseSetting(
                "Frontend:Origin",
                "http://localhost:3000");

            builder.UseSetting(
                "Resend:ApiKey",
                "fake-test-key");




            builder.ConfigureServices(services =>
            {

                var descriptor = services.RemoveAll<DbContextOptions<AppDbContext>>();

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(_connectionString)
                           .UseSnakeCaseNamingConvention();

                });

                //Replace Authentication
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;

                }).AddScheme<
                    AuthenticationSchemeOptions,
                    TestAuthHandler>
                    (TestAuthHandler.SchemeName, Options => { });

                // Replace external email provider
                services.RemoveAll<IEmailService>();
                services.AddSingleton<IEmailService>(_emailService);

                // Replace external object storage
                services.RemoveAll<IObjectStorage>();
                services.AddSingleton<IObjectStorage>(_objectStorage);

            });
        }

    }
}
