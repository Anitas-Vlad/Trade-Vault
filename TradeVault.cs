using TradeVault.Interfaces;

namespace TradeVault;

public class TradeVault : ITradeVault
{
    private readonly ICandlesRepository _candlesRepository;
    private readonly ITelegramService _telegramService;
    private readonly IBinanceService _binanceService;
    private readonly ICandleTracker _candleTracker;
    private readonly IBinanceCandleTracker _binanceCandleTracker;
    private readonly ILowHighTracker _lowHighTracker;

    private bool _isTradeVaultRunning = true;

    public TradeVault(ICandlesRepository candlesRepository,
        ITelegramService telegramService,
        IBinanceService binanceService, ICandleTracker candleTracker, IBinanceCandleTracker binanceCandleTracker, ILowHighTracker lowHighTracker)
    {
        
        _telegramService = telegramService;
        _binanceService = binanceService;
        _candleTracker = candleTracker;
        _candlesRepository = candlesRepository;
        _binanceCandleTracker = binanceCandleTracker;
        _lowHighTracker = lowHighTracker;
    }

    public async Task Run()
    {
        Console.WriteLine("App Running");
        await _telegramService.SendMessageAsync("App Running.");

        await _telegramService.InitializeLastUpdateId();

        while (_isTradeVaultRunning)
        {
            var message = await _telegramService.ListenForCommands();

            try
            {
                switch (message)
                {
                    case (null): break;
                    case "clear candles": // (Development)
                    {
                        await _candlesRepository.ClearCandles();
                        break;
                    }
                    case "stop binance":
                    {
                        _binanceCandleTracker.StopAll();
                        break;
                    }
                    default:
                        if (message.StartsWith("current"))
                        {
                            var currencyPrice = await _binanceService.GetCurrentPriceFromMessageAsync(message);
                            await _telegramService.SendMessageAsync($"{message}: {currencyPrice}");
                        }
                        else if (message.StartsWith("binance "))
                            await _binanceCandleTracker.AddAndStartCandleProcessorAsync(message);
                    
                        else if (message.StartsWith("lh "))
                            await _lowHighTracker.AddAndStartAsync(message);

                        else if (message.StartsWith("stop lh ")) //TODO Complete
                        {
                        
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                await _telegramService.SendMessageAsync("Error: " + e.Message);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}