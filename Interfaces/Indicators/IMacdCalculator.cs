using TradeVault.Models.Helpers;
using TradeVault.Services.Indicators.Results;

namespace TradeVault.Interfaces.Indicators;

public interface IMacdCalculator
{
    MacdResult CalculateMacd(List<decimal> prices, int shortPeriod, int longPeriod, int signalPeriod);
}