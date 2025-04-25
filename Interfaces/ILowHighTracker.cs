namespace TradeVault.Interfaces;

public interface ILowHighTracker
{
    Task AddLowHighTracker(string message);
    // void AddProcessor(string symbol, decimal lowPrice, decimal highPrice);
    Task StopLowHighTracker(string message);
}