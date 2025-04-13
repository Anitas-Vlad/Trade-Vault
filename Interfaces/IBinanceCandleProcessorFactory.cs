using TradeVault.Services.BinanceTracking;

namespace TradeVault.Interfaces;

public interface IBinanceCandleProcessorFactory
{
    BinanceCandleProcessor Create(string symbol, string timeSpan);
}