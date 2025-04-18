using TradeVault.Models.Enums;

namespace TradeVault.Interfaces.Indicators;

public interface ITradingSignalDetectorService
{
    Task<TradeSignal> GetTradeSignalAsync(string symbol, string interval);
}