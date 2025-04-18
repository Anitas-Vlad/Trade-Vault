namespace TradeVault.Interfaces.Indicators;

public interface IEmaCalculator
{
    List<decimal> CalculateEma(List<decimal> prices, int period);
}