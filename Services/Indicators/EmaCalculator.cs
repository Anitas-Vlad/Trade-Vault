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

        for (int i = period; i < prices.Count; i++)
        {
            decimal ema = ((prices[i] - emaPrev) * multiplier) + emaPrev;
            emaValues.Add(ema);
            emaPrev = ema;
        }

        return emaValues;
    }
}