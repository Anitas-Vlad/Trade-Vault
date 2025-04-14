using TradeVault.Interfaces;

namespace TradeVault.Services.LowHighTracking;

public class LowHighProcessorFactory : ILowHighProcessorFactory
{
    private readonly ITelegramService _telegramService;
    private readonly IInputParserService _inputParserService;
    private readonly INotificationsService _notificationsService;
    private readonly IBinanceService _binanceService;

    public LowHighProcessorFactory(ITelegramService telegramService, IInputParserService inputParserService,
        INotificationsService notificationsService, IBinanceService binanceService)
    {
        _telegramService = telegramService;
        _inputParserService = inputParserService;
        _notificationsService = notificationsService;
        _binanceService = binanceService;
    }

    public LowHighProcessor Create(string symbol, decimal lowPrice, decimal highPrice)
        => new(symbol, lowPrice, highPrice, _telegramService, _inputParserService, _notificationsService,
            _binanceService);
}