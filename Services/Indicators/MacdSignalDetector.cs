using TradeVault.Interfaces.Indicators;
using TradeVault.Models.Enums;
using TradeVault.Responses;
using TradeVault.Services.Indicators.Results;

namespace TradeVault.Services.Indicators;

public class MacdSignalDetector : IMacdSignalDetector
{
    private readonly IEmaCalculator _emaCalculator;
    private readonly IRsiCalculator _rsiCalculator;

    public MacdSignalDetector(IEmaCalculator emaCalculator, IRsiCalculator rsiCalculator)
    {
        _emaCalculator = emaCalculator;
        _rsiCalculator = rsiCalculator;
    }
    
    public MacdResult CalculateMacd(List<decimal> prices, int shortPeriod, int longPeriod, int signalPeriod)
    {
        var shortEma = _emaCalculator.CalculateEma(prices, shortPeriod);
        var longEma = _emaCalculator.CalculateEma(prices, longPeriod);

        var offset = longEma.Count - shortEma.Count;
        var macdLine = shortEma.Skip(offset).Zip(longEma, (s, l) => s - l).ToList();
        var signalLine = _emaCalculator.CalculateEma(macdLine, signalPeriod);
        var histogram = macdLine.Skip(macdLine.Count - signalLine.Count)
            .Zip(signalLine, (m, s) => m - s)
            .ToList();

        return new MacdResult
        {
            MacdLine = macdLine,
            SignalLine = signalLine,
            Histogram = histogram
        };
    }
    
    public TradeSignal CheckMacdSignal(List<BinanceKlineResponse> candles, int shortPeriod, int longPeriod, int signalPeriod)
    {
        var closePrices = candles.Select(c => c.Close).ToList();
        var macd = CalculateMacd(closePrices, shortPeriod, longPeriod, signalPeriod);
        var rsi = _rsiCalculator.CalculateRsi(closePrices);

        if (macd.SignalLine.Count < 2 || rsi.Values.Count < 2) return TradeSignal.Default;

        var macdLast = macd.MacdLine.Last();
        var macdPrev = macd.MacdLine[^2];
        var signalLast = macd.SignalLine.Last();
        var signalPrev = macd.SignalLine[^2];
        var rsiLast = rsi.Values.Last();

        bool macdCrossedUp = macdPrev < signalPrev && macdLast > signalLast;
        bool macdCrossedDown = macdPrev > signalPrev && macdLast < signalLast;

        if (macdCrossedUp && rsiLast < 30)
            return TradeSignal.Buy;

        if (macdCrossedDown && rsiLast > 70)
            return TradeSignal.Sell;

        return TradeSignal.Default;
    }
}