using TradeVault.Context;
using TradeVault.Interfaces;

namespace TradeVault.Services.Tracking;

public class CandleProcessorFactory : ICandleProcessorFactory
{
    private readonly IBinanceService _binanceService;
    private readonly ITelegramService _telegramService;
    private readonly ICoinsRepository _coinsRepository;
    private readonly ICandlesRepository _candlesRepository;
    private readonly TradeVaultContext _tradeVaultContext;
    private readonly IAlgorithmService _algorithmService;

    public CandleProcessorFactory(TradeVaultContext context, ITelegramService telegramService,
        IAlgorithmService algorithmService, IBinanceService binanceService, ICoinsRepository coinsRepository,
        ICandlesRepository candlesRepository)
    {
        _tradeVaultContext = context;
        _telegramService = telegramService;
        _binanceService = binanceService;
        _coinsRepository = coinsRepository;
        _candlesRepository = candlesRepository;
        _algorithmService = algorithmService;
    }

    public CandleProcessor Create(string symbol, int secondsTimeSpan)
    {
        return new CandleProcessor(_tradeVaultContext, _telegramService, _algorithmService, _binanceService,
            _coinsRepository, _candlesRepository, symbol, secondsTimeSpan);
    }
}