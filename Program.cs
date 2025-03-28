using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TradeVault.Context;
using TradeVault.Context.Repositories;
using TradeVault.Interfaces;
using TradeVault.Services;
using TradeVault.Services.Mappers;

using var host = CreateHostBuilder(args).Build();

var tradeVault = host.Services.GetRequiredService<ITradeVault>();

await tradeVault.Run();

return;

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config.AddEnvironmentVariables();
        })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders(); // Remove all default log providers
            logging.AddConsole(); // Only use Console logging
        })
        .ConfigureServices((hostContext, services) =>
        {
            var configuration = hostContext.Configuration;
            services.AddSingleton<HttpClient>();

            services.AddSingleton<ITradeVault, TradeVault.TradeVault>();
            services.AddScoped<IBtcPriceService, BtcPriceService>();
            services.AddScoped<ITelegramService, TelegramService>();
            services.AddScoped<IBinanceService, BinanceService>();
            services.AddScoped<ICoinMapper, CoinMapper>();
            services.AddScoped<ICandlesRepository, CandlesRepository>();
            services.AddScoped<ICoinsRepository, CoinsRepository>();
            services.AddScoped<ICandleProcessorFactory, CandleProcessorFactory>();
            services.AddScoped<ICandleProcessor, CandleProcessor>();
            services.AddScoped<ICandleTracker, CandleTracker>();
            services.AddScoped<ICandleTrackingService, CandleTrackingService>();
            
            var connectionString = configuration.GetConnectionString("trade-vault-context");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'trade-vault-context' not found.");

            services.AddDbContext<TradeVaultContext>(options =>
                options.UseSqlServer(connectionString));
        });
}