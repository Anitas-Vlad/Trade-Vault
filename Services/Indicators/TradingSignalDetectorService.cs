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

        // var closePrices = candles.Select(c => c.Close).ToList();
        // var volumes = candles.Select(c => c.Volume).ToList();

        // MACD (using your custom 6,13,9 config)
        // var macd = _macdCalculator.CalculateMacd(closePrices, 6, 13, 9);

        // RSI (choose 7 or 8 to match MACD speed)
        // var rsi = _rsiCalculator.CalculateRsi(closePrices, 7);

        // Volume Spike
        // var volumeSpike = _volumeSpikeDetector.Analyze(volumes);

        // Evaluate all indicators and return final signal
        return _signalEvaluator.Evaluate(candles, 6,13,9,8);
    }
}