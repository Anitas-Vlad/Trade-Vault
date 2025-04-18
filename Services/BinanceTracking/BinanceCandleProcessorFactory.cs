using TradeVault.Interfaces;
using TradeVault.Interfaces.Indicators;

namespace TradeVault.Services.BinanceTracking;

public class BinanceCandleProcessorFactory : IBinanceCandleProcessorFactory
{
    private readonly IBinanceService _binanceService;
    private readonly ITelegramService _telegramService;
    private readonly IAlgorithmService _algorithmService;
    private readonly IMacdSignalDetector _macdSignalDetector;
    private readonly ITradingSignalDetectorService _tradingSignalDetector;

    public BinanceCandleProcessorFactory(IBinanceService binanceService, ITelegramService telegramService,
        IAlgorithmService algorithmService, IMacdSignalDetector macdSignalDetector,
        ITradingSignalDetectorService tradingSignalDetector)
    {
        _binanceService = binanceService;
        _telegramService = telegramService;
        _algorithmService = algorithmService;
        _macdSignalDetector = macdSignalDetector;
        _tradingSignalDetector = tradingSignalDetector;
    }

    public BinanceCandleProcessor Create(string symbol, string timeSpan)
        => new(_binanceService, _telegramService, _algorithmService, _macdSignalDetector, _tradingSignalDetector,
            symbol, timeSpan);
}