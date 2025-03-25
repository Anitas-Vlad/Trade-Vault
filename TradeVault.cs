using TradeVault.Interfaces;

namespace TradeVault;

public class TradeVault : ITradeVault
{
    private readonly IBtcPriceService _btcPriceService;
    private readonly ITelegramService _telegramService;
    private readonly IBinanceService _binanceService;
    
    public TradeVault(IBtcPriceService btcPriceService, ITelegramService telegramService, IBinanceService binanceService)
    {
        _btcPriceService = btcPriceService;
        _telegramService = telegramService;
        _binanceService = binanceService;
    }

    public async Task Run()
    {
        Console.WriteLine("App Running");
        await _telegramService.SendMessageAsync("App Running.");
        
        await _telegramService.InitializeLastUpdateId();

        while (true)
        {
            var message = await _telegramService.ListenForCommands();

            if (message!.StartsWith("current"))
            {
                var currencyPrice = await _binanceService.GetCurrencyPriceAsync(message);
                await _telegramService.SendMessageAsync($"{message}: {currencyPrice}");
            }
        }
    }
}