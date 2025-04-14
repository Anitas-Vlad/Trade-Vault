using TradeVault.Responses;

namespace TradeVault.Interfaces;

public interface IBinanceService
{
    Task<decimal> GetCurrentPriceFromMessageAsync(string message);
    Task<decimal> GetCurrentPriceForSymbol(string symbol);
    Task<List<BinanceKlineResponse>> FetchCandlesAsync(string symbol, string interval, int limit = 150);
    Task<BinanceKlineResponse> FetchLastCandleAsync(string symbol, string interval);
}