using TradeVault.Interfaces;

namespace TradeVault;

public class TradeVault : ITradeVault
{
    private readonly IBtcPriceService _btcPriceService;
    private readonly ITelegramService _telegramService;
    private readonly IBinanceService _binanceService;
    private readonly ICandleTracker _candleTracker;

    public TradeVault(IBtcPriceService btcPriceService, ITelegramService telegramService,
        IBinanceService binanceService, ICandleTracker candleTracker)
    {
        _btcPriceService = btcPriceService;
        _telegramService = telegramService;
        _binanceService = binanceService;
        _candleTracker = candleTracker;
    }

    public async Task RefreshDb()
    {
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
                default:
                    if (message!.StartsWith("current"))
                    {
                        var currencySymbol = message.Replace("current ", "");
                        var currencyPrice = await _binanceService.GetCurrencyPriceAsync(currencySymbol);
                        await _telegramService.SendMessageAsync($"{message}: {currencyPrice}");
                    }

                    break;
            }
        }
    }
}