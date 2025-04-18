using TradeVault.Interfaces.Indicators;
using TradeVault.Models.Helpers;
using TradeVault.Services.Indicators.Results;

namespace TradeVault.Services.Indicators;

public class MacdCalculator : IMacdCalculator
{
    private readonly IEmaCalculator _emaCalculator;

    public MacdCalculator(IEmaCalculator emaCalculator)
    {
        _emaCalculator = emaCalculator;
    }
    
    public MacdResult CalculateMacd(List<decimal> prices, int shortPeriod, int longPeriod, int signalPeriod)
    {
        var shortEma = _emaCalculator.CalculateEma(prices, shortPeriod);
        var longEma = _emaCalculator.CalculateEma(prices, longPeriod);

        int offset = longEma.Count - shortEma.Count;
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
}