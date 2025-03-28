namespace TradeVault.Interfaces;

public interface ICandleTracker
{
    void AddProcessor(string symbol, int secondsTimeSpan);
    Task StartAllAsync();
    void StopAll();
}