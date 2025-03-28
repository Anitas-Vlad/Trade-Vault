using TradeVault.Models;

namespace TradeVault.Interfaces;

public interface ICandleProcessor
{
    Task StartProcessingAsync();
    void StopProcessing();
}