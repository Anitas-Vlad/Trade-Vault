using TradeVault.Models.Enums;
using TradeVault.Responses;
using TradeVault.Services.Indicators.Results;

namespace TradeVault.Interfaces.Indicators;

public interface IMacdSignalDetector
{
    MacdResult CalculateMacd(List<decimal> prices, int shortPeriod, int longPeriod, int signalPeriod);

    TradeSignal CheckMacdSignal(List<BinanceKlineResponse> candles, int shortPeriod, int longPeriod,
        int signalPeriod);
}