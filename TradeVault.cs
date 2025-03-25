using TelegramBitcoinPrices.Interfaces;
using TradeVault.Interfaces;

namespace TradeVault;

public class TradeVault : ITradeVault
{
    private readonly IBtcPriceService _btcPriceService;
    private readonly ITelegramService _telegramService;
    
    public TradeVault(IBtcPriceService btcPriceService, ITelegramService telegramService)
    {
        _btcPriceService = btcPriceService;
        _telegramService = telegramService;
    }

    public async Task Run()
    {
        await _telegramService.InitializeLastUpdateId();
        
        await _btcPriceService.GetBitcoinPriceAsync();

        while (true)
        {
            var message = await _telegramService.ListenForCommands();

            switch (message)
            {
                case "btceur":
                    await _btcPriceService.GetBitcoinPriceAsync();
                    break;
            }
        }
    }
}