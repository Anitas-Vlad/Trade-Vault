namespace TradeVault.Interfaces;

public interface ILowHighTracker
{
    Task AddAndStartAsync(string message);
    // void AddProcessor(string symbol, decimal lowPrice, decimal highPrice);
    Task StopProcessor(string symbol);
}