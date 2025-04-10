using TradeVault.Responses;

namespace TradeVault.Interfaces;

public interface ICandleTracker
{
    void AddProcessor(string symbol, int secondsTimeSpan);
    Task StartAllAsync();
    Task<CandleProcessorInfo> AddAndStartCandleProcessorAsync(string message);
    void StopAll();
}