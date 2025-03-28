using TradeVault.Interfaces;

namespace TradeVault.Services;

public class CandleProcessorFactory : ICandleProcessorFactory
{
    private readonly IBinanceService _binanceService;
    private readonly ICoinsRepository _coinsRepository;
    private readonly ICandlesRepository _candlesRepository;

    public CandleProcessorFactory(IBinanceService binanceService, ICoinsRepository coinsRepository, ICandlesRepository candlesRepository)
    {
        _binanceService = binanceService;
        _coinsRepository = coinsRepository;
        _candlesRepository = candlesRepository;
    }
    
    public CandleProcessor Create(string symbol, int secondsTimeSpan)
    {
        return new CandleProcessor(_binanceService, _coinsRepository, _candlesRepository, symbol, secondsTimeSpan);
    }
}