using TradeVault.Responses;

namespace TradeVault.Interfaces;

public interface IBinanceCandleProcessor
{
    BinanceCandleProcessorInfo GetInfo();
    Task StartProcessingAsync();
    void StopProcessing();
}