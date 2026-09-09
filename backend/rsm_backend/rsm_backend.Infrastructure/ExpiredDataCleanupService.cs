using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rsm_backend.Infrastructure.Data;

namespace rsm_backend.Infrastructure
{
    public class ExpiredDataCleanupService:BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ExpiredDataCleanupService(IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(5));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                using var scope = _scopeFactory.CreateScope();

                var db = scope.ServiceProvider
                            .GetRequiredService<AppDbContext>();

                await db.GuestOrderVerifications
                    .Where(x => x.ExpiresAt <= DateTimeOffset.UtcNow)
                    .ExecuteDeleteAsync(stoppingToken);
            }
        }
    }
}
