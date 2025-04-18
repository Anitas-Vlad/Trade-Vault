using TradeVault.Interfaces.Indicators;
using TradeVault.Services.Indicators.Results;

namespace TradeVault.Services.Indicators;

public class RsiCalculator : IRsiCalculator
{
    public RsiResult CalculateRsi(List<decimal> prices, int period)
    {
        var result = new RsiResult();

        if (prices.Count < period + 1)
            return result;

        decimal gain = 0, loss = 0;
        for (var i = 1; i <= period; i++)
        {
            var change = prices[i] - prices[i - 1];
            if (change >= 0) gain += change;
            else loss -= change;
        }

        var avgGain = gain / period;
        var avgLoss = loss / period;

        result.Values.Add(100 - (100 / (1 + (avgGain / avgLoss))));

        for (var i = period + 1; i < prices.Count; i++)
        {
            var change = prices[i] - prices[i - 1];
            gain = change > 0 ? change : 0;
            loss = change < 0 ? -change : 0;

            avgGain = ((avgGain * (period - 1)) + gain) / period;
            avgLoss = ((avgLoss * (period - 1)) + loss) / period;

            var rs = avgLoss == 0 ? 100 : avgGain / avgLoss;
            result.Values.Add(100 - (100 / (1 + rs)));
        }

        return result;
    }
}