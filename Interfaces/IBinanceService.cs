namespace TradeVault.Interfaces;

public interface IBinanceService
{
    Task<decimal> GetCurrencyPriceAsync(string message);
}