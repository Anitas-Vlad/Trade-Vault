using TradeVault.Interfaces;
using TradeVault.Interfaces.Indicators;
using TradeVault.Models.Enums;

namespace TradeVault.Services.Indicators;

public class TradingSignalDetectorService : ITradingSignalDetectorService
{
    private readonly IBinanceService _binanceService;
    private readonly IMacdCalculator _macdCalculator;
    private readonly IRsiCalculator _rsiCalculator;
    // private readonly IVolumeAnalyzer _volumeSpikeDetector;
    private readonly ISignalEvaluator _signalEvaluator;

    public TradingSignalDetectorService(
        IBinanceService binanceService,
        IMacdCalculator macdCalculator,
        IRsiCalculator rsiCalculator,
        // IVolumeAnalyzer volumeSpikeDetector,
        ISignalEvaluator signalEvaluator)
    {
        _binanceService = binanceService;
        _macdCalculator = macdCalculator;
        _rsiCalculator = rsiCalculator;
        // _volumeSpikeDetector = volumeSpikeDetector;
        _signalEvaluator = signalEvaluator;
    }
    
    public async Task<TradeSignal> GetTradeSignalAsync(string symbol, string interval)
    {
        var candles = await _binanceService.FetchCandlesAsync(symbol, interval);
        if (candles == null || candles.Count < 20)
            throw new Exception("Not enough data to calculate indicators.");
        
        return _signalEvaluator.Evaluate(candles, 6,13,9,8);
    }
}