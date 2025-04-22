using TradeVault.Interfaces.Indicators;

namespace TradeVault.Services.Indicators;

public class EmaCalculator : IEmaCalculator
{
    public List<decimal> CalculateEma(List<decimal> prices, int period)
    {
        var emaValues = new List<decimal>();
        if (prices.Count < period) return emaValues;

        var multiplier = 2m / (period + 1);
        var emaPrev = prices.Take(period).Average();
        emaValues.Add(emaPrev);

        for (var i = period; i < prices.Count; i++)
        {
            var price = prices[i];
            var ema = (price - emaPrev) * multiplier + emaPrev;
            emaValues.Add(ema);
            emaPrev = ema;
        }

        return emaValues;
    }
}