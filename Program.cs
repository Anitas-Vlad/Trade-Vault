using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TradeVault.Context;
using TradeVault.Context.Repositories;
using TradeVault.Interfaces;
using TradeVault.Services;
using TradeVault.Services.BinanceTracking;
using TradeVault.Services.Input;
using TradeVault.Services.LowHighTracking;
using TradeVault.Services.Mappers;
using TradeVault.Services.Tracking;

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
            services.AddScoped<ITelegramService, TelegramService>();
            services.AddScoped<IBinanceService, BinanceService>();
            services.AddScoped<ICoinMapper, CoinMapper>();
            services.AddScoped<ICandlesRepository, CandlesRepository>();
            services.AddScoped<ICoinsRepository, CoinsRepository>();
            services.AddScoped<ICandleProcessorFactory, CandleProcessorFactory>();
            services.AddScoped<ICandleProcessor, CandleProcessor>();
            services.AddScoped<ICandleTracker, CandleTracker>();
            services.AddScoped<IInputValidator, InputValidator>();
            services.AddScoped<IAlgorithmService, AlgorithmService>();
            services.AddScoped<IBinanceCandleProcessor, BinanceCandleProcessor>();
            services.AddScoped<IBinanceCandleProcessorFactory, BinanceCandleProcessorFactory>();
            services.AddScoped<IBinanceCandleTracker, BinanceCandleTracker>();
            
            services.AddSingleton<INotificationsService, NotificationsService>();
            services.AddSingleton<ILowHighProcessorFactory, LowHighProcessorFactory>();
            services.AddSingleton<ILowHighProcessor, LowHighProcessor>();
            services.AddSingleton<ILowHighTracker, LowHighTracker>();
            services.AddScoped<IInputParserService, InputParserService>();
            
            // services.AddScoped<IBinanceCandleFetcher, BinanceCandleFetcher>();
            
            var connectionString = configuration.GetConnectionString("trade-vault-context");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'trade-vault-context' not found.");

            services.AddDbContext<TradeVaultContext>(options =>
                options.UseSqlServer(connectionString));
        });
}