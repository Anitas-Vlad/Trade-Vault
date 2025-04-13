using TradeVault.Responses;

namespace TradeVault.Interfaces;

public interface IBinanceService
{
    Task<decimal> GetCurrencyPriceAsync(string message);
    Task<List<BinanceKlineResponse>> FetchCandlesAsync(string symbol, string interval, int limit = 150);
    Task<BinanceKlineResponse> FetchLastCandleAsync(string symbol, string interval);
}