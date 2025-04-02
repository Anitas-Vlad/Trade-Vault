using TradeVault.Context;
using TradeVault.Interfaces;

namespace TradeVault.Services;

public class CandleProcessorFactory : ICandleProcessorFactory
{
    private readonly IBinanceService _binanceService;
    private readonly ICoinsRepository _coinsRepository;
    private readonly ICandlesRepository _candlesRepository;
    private readonly TradeVaultContext _tradeVaultContext;

    public CandleProcessorFactory(TradeVaultContext context, IBinanceService binanceService, ICoinsRepository coinsRepository, ICandlesRepository candlesRepository)
    {
        _tradeVaultContext = context;
        _binanceService = binanceService;
        _coinsRepository = coinsRepository;
        _candlesRepository = candlesRepository;
    }
    
    public CandleProcessor Create(string symbol, int secondsTimeSpan)
    {
        return new CandleProcessor(_tradeVaultContext, _binanceService, _coinsRepository, _candlesRepository, symbol, secondsTimeSpan);
    }
}