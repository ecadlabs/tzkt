using System;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Tzkt.Data;
using Tzkt.Sync.Services;

namespace Tzkt.Sync
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args).ConfigureIndexer().Build().Init().Run();
        }
    }

    public static class IHostBuilderExt
    {
        public static IHostBuilder ConfigureIndexer(this IHostBuilder host) => host
            .ConfigureHostConfiguration(configHost =>
            {
                configHost.AddEnvironmentVariables("TZKT_");
            })
            .ConfigureAppConfiguration((hostContext, configApp) =>
            {
                configApp.AddEnvironmentVariables("TZKT_");
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<TzktContext>(options =>
                    options.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection")));

                services.AddCaches();
                services.AddTezosNode();
                services.AddTezosProtocols();
                services.AddQuotes(hostContext.Configuration);
                services.AddHostedService<Observer>();
            });
    }

    public static class IHostExt
    {
        public static IHost Init(this IHost host, int attempt = 0)
        {
            using var scope = host.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var db = scope.ServiceProvider.GetRequiredService<TzktContext>();

            try
            {
                logger.LogInformation("Initialize database");

                var migrations = db.Database.GetPendingMigrations();
                if (migrations.Any())
                {
                    logger.LogWarning($"{migrations.Count()} database migrations were found. Applying migrations...");
                    db.Database.Migrate();
                }

                logger.LogInformation("Database initialized");
                return host;
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Failed to initialize database: {ex.Message}. Try again...");
                if (attempt >= 10) throw;
                Thread.Sleep(1000);

                return host.Init(++attempt);
            }
        }
    }
}
