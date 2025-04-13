using TradeVault.Interfaces;

namespace TradeVault.Services.BinanceTracking;

public class BinanceCandleProcessorFactory : IBinanceCandleProcessorFactory
{
    private readonly IBinanceService _binanceService;
    private readonly ITelegramService _telegramService;
    private readonly IAlgorithmService _algorithmService;

    public BinanceCandleProcessorFactory(IBinanceService binanceService, ITelegramService telegramService,
        IAlgorithmService algorithmService)
    {
        _binanceService = binanceService;
        _telegramService = telegramService;
        _algorithmService = algorithmService;
    }

    public BinanceCandleProcessor Create(string symbol, string timeSpan)
        => new(_binanceService, _telegramService, _algorithmService, symbol, timeSpan);
}