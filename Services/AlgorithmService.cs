using TradeVault.Interfaces;
using TradeVault.Models.Enums;
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

    private MacdResult CalculateMacd(List<decimal> prices, int shortPeriod = 12, int longPeriod = 26,
            int signalPeriod = 9) //TODO Edit 
        //TODO use it whenever creating a new candle.
    {
        var shortEma = CalculateEma(prices, shortPeriod);
        var longEma = CalculateEma(prices, longPeriod);

        var offset = longEma.Count - shortEma.Count;
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

    public MacdResponseType CheckMacdSignal(List<decimal> prices, int shortPeriod, int longPeriod, int signalPeriod,
        string currencySymbol)
    {
        var macdResult = CalculateMacd(prices, shortPeriod, longPeriod, signalPeriod);

        if (macdResult.MacdLine.Count < 2 || macdResult.SignalLine.Count < 2)
            return MacdResponseType.Default;

        var lastIndex = macdResult.MacdLine.Count - 1;
        if (lastIndex - 1 < 0)
            return MacdResponseType.Default;

        var prevMacd = macdResult.MacdLine[lastIndex - 1];
        var prevSignal = macdResult.SignalLine[lastIndex - 1];
        var currMacd = macdResult.MacdLine[lastIndex];
        var currSignal = macdResult.SignalLine[lastIndex];

        // Buy when MACD crosses below the signal line
        if (prevMacd > prevSignal && currMacd < currSignal)
            return MacdResponseType.Buy;

        // Sell when MACD crosses above the signal line
        if (prevMacd < prevSignal && currMacd > currSignal)
            return MacdResponseType.Sell;

        return MacdResponseType.Default;
    }
}