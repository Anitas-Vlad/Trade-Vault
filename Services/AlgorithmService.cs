using TradeVault.Interfaces;
using TradeVault.Models;
using TradeVault.Models.Enums;
using TradeVault.Models.Helpers;
using TradeVault.Responses;
using TradeVault.Services.Indicators.Results;

namespace TradeVault.Services;

public class AlgorithmService : IAlgorithmService
{
    public List<decimal> CalculateEma(List<decimal> prices, int period)
    {
        var emaValues = new List<decimal>();

        if (prices.Count < period)
            return emaValues;

        var multiplier = 2m / (period + 1);
        var emaPrev = prices.Take(period).Average(); // SMA to start

        emaValues.Add(emaPrev);

        for (var i = period; i < prices.Count; i++)
        {
            var price = prices[i];
            var ema = ((price - emaPrev) * multiplier) + emaPrev;
            emaValues.Add(ema);
            emaPrev = ema;
        }

        return emaValues;
    }

    private MacdResult CalculateMacd(List<decimal> prices, int shortPeriod, int longPeriod, int signalPeriod = 9) 
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

    public TradeSignal CheckMacdSignal(List<BinanceKlineResponse> candles, int shortPeriod, int longPeriod, int signalPeriod,
        string currencySymbol)
    {
        var candlesCloseValues = candles.Select(candle => candle.Close).ToList();
        
        var macdResult = CalculateMacd(candlesCloseValues, shortPeriod, longPeriod, signalPeriod);

        if (macdResult.MacdLine.Count < 2 || macdResult.SignalLine.Count < 2)
            return TradeSignal.Default;

        var lastIndex = macdResult.SignalLine.Count - 1;
        if (lastIndex - 1 < 0)
            return TradeSignal.Default;

        var prevMacd = macdResult.MacdLine[lastIndex - 1];
        var prevSignal = macdResult.SignalLine[lastIndex - 1];
        var currMacd = macdResult.MacdLine[lastIndex];
        var currSignal = macdResult.SignalLine[lastIndex];

        // ✅ Buy when MACD crosses above the Signal Line
        if (prevMacd < prevSignal && currMacd > currSignal)
            return TradeSignal.Buy;

        // ✅ Sell when MACD crosses below the Signal Line
        if (prevMacd > prevSignal && currMacd < currSignal)
            return TradeSignal.Sell;

        return TradeSignal.Default;
    }

    public TradeSignal CheckMacdSignal(List<decimal> candlesClosingPrices, int shortPeriod, int longPeriod, int signalPeriod,
        string currencySymbol) //Refactor or Delete. This is a duplicate
    {
        
        var macdResult = CalculateMacd(candlesClosingPrices, shortPeriod, longPeriod, signalPeriod);

        if (macdResult.MacdLine.Count < 2 || macdResult.SignalLine.Count < 2)
            return TradeSignal.Default;

        var lastIndex = macdResult.SignalLine.Count - 1;
        if (lastIndex - 1 < 0)
            return TradeSignal.Default;

        var prevMacd = macdResult.MacdLine[lastIndex - 1];
        var prevSignal = macdResult.SignalLine[lastIndex - 1];
        var currMacd = macdResult.MacdLine[lastIndex];
        var currSignal = macdResult.SignalLine[lastIndex];

        // ✅ Buy when MACD crosses above the Signal Line
        if (prevMacd < prevSignal && currMacd > currSignal)
            return TradeSignal.Buy;

        // ✅ Sell when MACD crosses below the Signal Line
        if (prevMacd > prevSignal && currMacd < currSignal)
            return TradeSignal.Sell;

        return TradeSignal.Default;
    }
}