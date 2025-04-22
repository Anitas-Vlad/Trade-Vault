using TradeVault.Interfaces;
using TradeVault.Models.Enums;

namespace TradeVault.Services.LowHighTracking;

public class LowHighProcessor : ILowHighProcessor
{
    private readonly ITelegramService _telegramService;
    private readonly IInputParserService _inputParserService;
    private readonly INotificationsService _notificationsService;
    private readonly IBinanceService _binanceService;

    private readonly CancellationTokenSource _cts = new();

    private BtcPriceStatus _awaitedBtcPriceStatus = BtcPriceStatus.Skip;
    public readonly string Symbol;
    private decimal _currentPrice;
    private decimal _highPriceBorder;
    private decimal _lowPriceBorder;

    public LowHighProcessor(string symbol, decimal lowPrice, decimal highPrice,
        ITelegramService telegramService, IInputParserService inputParserService,
        INotificationsService notificationsService, IBinanceService binanceService)
    {
        _telegramService = telegramService;
        _inputParserService = inputParserService;
        _notificationsService = notificationsService;
        _binanceService = binanceService;

        Symbol = symbol;
        _lowPriceBorder = lowPrice;
        _highPriceBorder = highPrice;
    }

    public void Reset(decimal lowPrice, decimal highPrice)
    {
        _awaitedBtcPriceStatus = BtcPriceStatus.Skip;
        _currentPrice = 0;
        _lowPriceBorder = lowPrice;
        _highPriceBorder = highPrice;
    }

    public async Task Stop()
    {
        _cts.Cancel();
        await _telegramService.SendMessageAsync("Succesfully stopped LowHigh for " + Symbol);
    }

    public async Task StartLowHighNotifications(string input)
    {
        var token = _cts.Token;

        try
        {
            await _telegramService.SendMessageAsync("Successfully activated low high notifications...");

            _ = Task.Run(async () =>
            {
                await CheckCurrentPrice();

                switch (_awaitedBtcPriceStatus)
                {
                    case BtcPriceStatus.Buy:
                    {
                        await _notificationsService.SendBuyTargetAlert(_currentPrice);
                        break;
                    }
                    case BtcPriceStatus.Rise:
                    {
                        await _notificationsService.SendSellTargetAlert(_currentPrice);
                        break;
                    }
                    case BtcPriceStatus.Drop:
                    {
                        await _notificationsService.SendSellWarningAlert(_currentPrice);
                        break;
                    }
                    case BtcPriceStatus.Skip:
                        break;
                    default:
                        break;
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }, token);
        }
        catch (Exception e)
        {
            await _telegramService.SendMessageAsync($"Error: {e.Message}");
        }
    }

    private async Task CheckCurrentPrice()
    {
        _currentPrice = await _binanceService.GetCurrentPriceForSymbol(Symbol);
        Console.WriteLine($"Current {Symbol} Price: {_currentPrice}"); //TODO delete
        if (_currentPrice > _highPriceBorder)
        {
            _awaitedBtcPriceStatus = BtcPriceStatus.Rise;
            _highPriceBorder = _currentPrice;
            return;
        }

        if (_currentPrice < _lowPriceBorder)
        {
            _awaitedBtcPriceStatus = BtcPriceStatus.Drop;
            _lowPriceBorder = _currentPrice;
            return;
        }

        _awaitedBtcPriceStatus = BtcPriceStatus.Skip;
    }
}