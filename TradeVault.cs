using TradeVault.Interfaces;

namespace TradeVault;

public class TradeVault : ITradeVault
{
    private readonly IBtcPriceService _btcPriceService;
    private readonly ICandlesRepository _candlesRepository;
    private readonly ITelegramService _telegramService;
    private readonly IBinanceService _binanceService;
    private readonly ICandleTracker _candleTracker;

    public TradeVault(IBtcPriceService btcPriceService, ICandlesRepository candlesRepository,
        ITelegramService telegramService,
        IBinanceService binanceService, ICandleTracker candleTracker)
    {
        _btcPriceService = btcPriceService;
        _telegramService = telegramService;
        _binanceService = binanceService;
        _candleTracker = candleTracker;
        _candlesRepository = candlesRepository;
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
                case "clear candles": // (Development)
                {
                    await _candlesRepository.ClearCandles();
                    break;
                }
                default:
                    if (message!.StartsWith("current"))
                    {
                        var currencyPrice = await _binanceService.GetCurrencyPriceAsync(message);
                        await _telegramService.SendMessageAsync($"{message}: {currencyPrice}");
                    }
                    else if (message!.StartsWith("track "))
                    {
                        var processorInfo = await _candleTracker.AddAndStartProcessorAsync(message);
                        await _telegramService.SendMessageAsync(
                            $"Tracking candles for {processorInfo.Symbol}, for every {processorInfo.SecondsTimeSpan} seconds.");
                    }

                    break;
            }
        }
    }
}