using TradeVault.Responses;

namespace TradeVault.Interfaces;

public interface IBinanceCandleTracker
{
    void AddProcessor(string symbol, string timeSpan);
    Task<BinanceCandleProcessorInfo> AddAndStartCandleProcessorAsync(string message);
    Task StartAllAsync();
    void StopAll();
}