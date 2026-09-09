using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using rsm_backend.Application.Services.Interfaces.Infrastructure;
using rsm_backend.Infrastructure.Data;
using rsm_backend.Infrastructure.FakeServicesForTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Api
{
    public class EcommerceApiFactory:WebApplicationFactory<Program>
    {
        private readonly string _connectionString;
        public FakeObjectStorage _objectStorage { get; } = new();
        public FakeEmailService _emailService { get; } = new();

        public EcommerceApiFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                var descriptor = services.RemoveAll<DbContextOptions<AppDbContext>>();

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(_connectionString);

                });

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
