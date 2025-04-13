using TradeVault.Services;
using TradeVault.Services.Tracking;

namespace TradeVault.Interfaces;

public interface ICandleProcessorFactory
{
    CandleProcessor Create(string symbol, int secondsTimeSpan);
}