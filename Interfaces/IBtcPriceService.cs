namespace TradeVault.Interfaces;

public interface IBtcPriceService
{
    Task<decimal> GetBitcoinPriceAsync();
}