using TradeVault.Models.Enums;
using TradeVault.Responses;

namespace TradeVault.Interfaces.Indicators;

public interface ISignalEvaluator
{
    TradeSignal Evaluate(List<BinanceKlineResponse> candles, int shortEmaPeriod, int longEmaPeriod, int signalPeriod,
        int rsiPeriod);
}