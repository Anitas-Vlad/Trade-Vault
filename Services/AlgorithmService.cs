using TradeVault.Interfaces;
using TradeVault.Models.Helpers;

namespace TradeVault.Services;

public class AlgorithmService : IAlgorithmService
{
    public List<decimal> CalculateEma(List<decimal> prices, int period)
    {
        List<decimal> emaValues = new List<decimal>();

        if (prices.Count < period)
            return emaValues;

        var multiplier = 2m / (period + 1);
        var emaPrev = prices.Take(period).Average(); // SMA to start

        emaValues.Add(emaPrev);

        for (int i = period; i < prices.Count; i++)
        {
            var price = prices[i];
            var ema = ((price - emaPrev) * multiplier) + emaPrev;
            emaValues.Add(ema);
            emaPrev = ema;
        }

        return emaValues;
    }
    
    public MacdResult CalculateMacd(List<decimal> prices, int shortPeriod = 12, int longPeriod = 26, int signalPeriod = 9) //TODO Edit 
    {
        var shortEma = CalculateEma(prices, shortPeriod);
        var longEma = CalculateEma(prices, longPeriod);

        int offset = longEma.Count - shortEma.Count;
        var macdLine = shortEma.Skip(offset).Zip(longEma, (shortVal, longVal) => shortVal - longVal).ToList();
        var signalLine = CalculateEma(macdLine, signalPeriod);
        var histogram = macdLine.Skip(macdLine.Count - signalLine.Count)
            .Zip(signalLine, (macd, signal) => macd - signal)
            .ToList();

        return new MacdResult
        {
            MacdLine = macdLine,
            SignalLine = signalLine,
            Histogram = histogram
        };
    }
}