using TradeVault.Interfaces;

namespace TradeVault;

public class TradeVault : ITradeVault
{
    private readonly IBtcPriceService _btcPriceService;
    private readonly ICandlesRepository _candlesRepository;
    private readonly ITelegramService _telegramService;
    private readonly IBinanceService _binanceService;
    private readonly ICandleTracker _candleTracker;
    private readonly IBinanceCandleTracker _binanceCandleTracker;

    public TradeVault(IBtcPriceService btcPriceService, ICandlesRepository candlesRepository,
        ITelegramService telegramService,
        IBinanceService binanceService, ICandleTracker candleTracker, IBinanceCandleTracker binanceCandleTracker)
    {
        _btcPriceService = btcPriceService;
        _telegramService = telegramService;
        _binanceService = binanceService;
        _candleTracker = candleTracker;
        _candlesRepository = candlesRepository;
        _binanceCandleTracker = binanceCandleTracker;
    }

    public async Task Run()
    {
        Console.WriteLine("App Running");
        await _telegramService.SendMessageAsync("App Running.");

        await _telegramService.InitializeLastUpdateId();

        while (true)
        {
            var message = await _telegramService.ListenForCommands();

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
                        var currencyPrice = await _binanceService.GetCurrencyPriceAsync(message);
                        await _telegramService.SendMessageAsync($"{message}: {currencyPrice}");
                    }
                    else if (message.StartsWith("binance "))
                    {
                        var processorInfo = await _binanceCandleTracker.AddAndStartCandleProcessorAsync(message);
                        await _telegramService.SendMessageAsync(
                            $"Tracking Binance {processorInfo.Symbol}: {processorInfo.TimeSpan} candles.");
                    }
                    // else if (message.StartsWith("track "))
                    // {
                    //     var processorInfo = await _candleTracker.AddAndStartCandleProcessorAsync(message);
                    //     await _telegramService.SendMessageAsync(
                    //         $"Tracking {processorInfo.Symbol}: {processorInfo.SecondsTimeSpan}sec candles.");
                    // }

                    break;
            }
        }
    }
}