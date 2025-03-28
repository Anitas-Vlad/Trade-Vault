using TradeVault.Services;

namespace TradeVault.Interfaces;

public interface ICandleProcessorFactory
{
    CandleProcessor Create(string symbol, int secondsTimeSpan);
}