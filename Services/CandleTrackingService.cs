using TradeVault.Interfaces;
using TradeVault.Models;

namespace TradeVault.Services;

public class CandleTrackingService : ICandleTrackingService
{
    private readonly IBinanceService _binanceService;
    private CancellationTokenSource _cts = new ();

    public CandleTrackingService(IBinanceService binanceService)
    {
        _binanceService = binanceService;
    }
}