using TradeVault.Services.Indicators.Results;

namespace TradeVault.Interfaces.Indicators;

public interface IRsiCalculator
{
    RsiResult CalculateRsi(List<decimal> prices, int period = 8);
}