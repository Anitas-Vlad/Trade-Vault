using TradeVault.Models.Helpers;

namespace TradeVault.Interfaces;

public interface IAlgorithmService
{
    List<decimal> CalculateEma(List<decimal> prices, int period);
    MacdResult CalculateMacd(List<decimal> prices, int shortPeriod, int longPeriod, int signalPeriod);
}