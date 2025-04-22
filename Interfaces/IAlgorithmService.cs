using TradeVault.Models.Enums;
using TradeVault.Models.Helpers;
using TradeVault.Responses;

namespace TradeVault.Interfaces;

public interface IAlgorithmService
{
    List<decimal> CalculateEma(List<decimal> prices, int period);
    // MacdResult CalculateMacd(List<decimal> prices, int shortPeriod, int longPeriod, int signalPeriod);
    TradeSignal CheckMacdSignal(List<BinanceKlineResponse> candles, int shortPeriod, int longPeriod, int signalPeriod,
        string currencySymbol);
    
    TradeSignal CheckMacdSignal(List<decimal> candlesClosingPrices, int shortPeriod, int longPeriod, int signalPeriod,
        string currencySymbol);
}