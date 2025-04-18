using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TradeVault.Context;
using TradeVault.Context.Repositories;
using TradeVault.Interfaces;
using TradeVault.Interfaces.Indicators;
using TradeVault.Services;
using TradeVault.Services.BinanceTracking;
using TradeVault.Services.Indicators;
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
            services.AddSingleton<ITelegramService, TelegramService>();
            services.AddSingleton<IBinanceService, BinanceService>();
            services.AddSingleton<ICoinMapper, CoinMapper>();
            services.AddSingleton<ICandlesRepository, CandlesRepository>();
            services.AddSingleton<ICoinsRepository, CoinsRepository>();
            services.AddSingleton<ICandleProcessorFactory, CandleProcessorFactory>();
            services.AddSingleton<ICandleProcessor, CandleProcessor>();
            services.AddSingleton<ICandleTracker, CandleTracker>();
            services.AddSingleton<IInputValidator, InputValidator>();
            services.AddSingleton<IAlgorithmService, AlgorithmService>();
            services.AddSingleton<IBinanceCandleProcessor, BinanceCandleProcessor>();
            services.AddSingleton<IBinanceCandleProcessorFactory, BinanceCandleProcessorFactory>();
            services.AddSingleton<IBinanceCandleTracker, BinanceCandleTracker>();
            
            services.AddSingleton<INotificationsService, NotificationsService>();
            services.AddSingleton<ILowHighProcessorFactory, LowHighProcessorFactory>();
            services.AddSingleton<ILowHighProcessor, LowHighProcessor>();
            services.AddSingleton<ILowHighTracker, LowHighTracker>();
            services.AddSingleton<IInputParserService, InputParserService>();

            services.AddSingleton<IEmaCalculator, EmaCalculator>();
            services.AddSingleton<IMacdCalculator, MacdCalculator>();
            services.AddSingleton<IMacdSignalDetector, MacdSignalDetector>();
            services.AddSingleton<IRsiCalculator, RsiCalculator>();
            services.AddSingleton<ISignalEvaluator, SignalEvaluator>();
            services.AddSingleton<IVolumeAnalyzer, VolumeAnalyzer>();
            services.AddSingleton<ITradingSignalDetectorService, TradingSignalDetectorService>();
            
            // services.AddScoped<IBinanceCandleFetcher, BinanceCandleFetcher>();
            
            var connectionString = configuration.GetConnectionString("trade-vault-context");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'trade-vault-context' not found.");

            services.AddDbContext<TradeVaultContext>(options =>
                options.UseSqlServer(connectionString));
        });
}