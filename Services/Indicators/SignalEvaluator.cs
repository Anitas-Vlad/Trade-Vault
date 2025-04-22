using TradeVault.Interfaces.Indicators;
using TradeVault.Models.Enums;
using TradeVault.Responses;

namespace TradeVault.Services.Indicators;

public class SignalEvaluator : ISignalEvaluator
{
    private readonly IMacdCalculator _macdCalculator;

    // private readonly IRsiCalculator _rsiCalculator;
    private readonly IVolumeAnalyzer _volumeAnalyzer;

    public SignalEvaluator(IMacdCalculator macdCalculator, IRsiCalculator rsiCalculator, IVolumeAnalyzer volumeAnalyzer)
    {
        _macdCalculator = macdCalculator;
        // _rsiCalculator = rsiCalculator;
        _volumeAnalyzer = volumeAnalyzer;
    }

    public TradeSignal Evaluate(List<BinanceKlineResponse> candles, int shortEmaPeriod, int longEmaPeriod,
        int signalPeriod, int rsiPeriod)
    {
        var closePrices = candles.Select(c => c.Close).ToList();

        var macdResult = _macdCalculator.CalculateMacd(closePrices, shortEmaPeriod, longEmaPeriod, signalPeriod);
        // var rsiValues = _rsiCalculator.CalculateRsi(closePrices, rsiPeriod);
        // var volumeAnalysis = _volumeAnalyzer.Analyze(candles);

        // if (macdResult.MacdLine.Count < 2 || macdResult.SignalLine.Count < 2 || rsiValues.Values.Count == 0)
        if (macdResult.MacdLine.Count < 2 || macdResult.SignalLine.Count < 2)
            return TradeSignal.Default;

        var lastIndex = macdResult.SignalLine.Count - 1;
        var prevMacd = macdResult.MacdLine[lastIndex - 1];
        var prevSignal = macdResult.SignalLine[lastIndex - 1];
        var currMacd = macdResult.MacdLine[lastIndex];
        var currSignal = macdResult.SignalLine[lastIndex];
        // var currentRsi = rsiValues.Values.Last();

        // // if (prevMacd < prevSignal && currMacd > currSignal && currentRsi > 50 && volumeAnalysis.IsVolumeIncreasing)
        // if (prevMacd < prevSignal && currMacd > currSignal && volumeAnalysis.IsVolumeIncreasing) // Inactive RSI
        //     // if (prevMacd < prevSignal && currMacd > currSignal && currentRsi > 50) // Inactive Volume
        //     return TradeSignal.Buy;
        //
        // // if (prevMacd > prevSignal && currMacd < currSignal && currentRsi < 50 && volumeAnalysis.IsVolumeIncreasing)
        // if (prevMacd > prevSignal && currMacd < currSignal && volumeAnalysis.IsVolumeIncreasing) // Inactive RSI
        //     // if (prevMacd > prevSignal && currMacd < currSignal && currentRsi < 50) // Inactive Volume
        //     return TradeSignal.Sell;
        
        // if (prevMacd < prevSignal && currMacd > currSignal && currentRsi > 50 && volumeAnalysis.IsVolumeIncreasing)
        if (prevMacd < prevSignal && currMacd > currSignal) // Inactive RSI/Volume
            // if (prevMacd < prevSignal && currMacd > currSignal && currentRsi > 50) // Inactive Volume
            return TradeSignal.Buy;

        // if (prevMacd > prevSignal && currMacd < currSignal && currentRsi < 50 && volumeAnalysis.IsVolumeIncreasing)
        if (prevMacd > prevSignal && currMacd < currSignal) // Inactive RSI/Volume
            // if (prevMacd > prevSignal && currMacd < currSignal && currentRsi < 50) // Inactive Volume
            return TradeSignal.Sell;

        return TradeSignal.Default;
    }
}