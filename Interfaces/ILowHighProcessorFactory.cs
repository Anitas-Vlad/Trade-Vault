using TradeVault.Services.LowHighTracking;

namespace TradeVault.Interfaces;

public interface ILowHighProcessorFactory
{
    public LowHighProcessor Create(string symbol, decimal lowPrice, decimal highPrice);
}