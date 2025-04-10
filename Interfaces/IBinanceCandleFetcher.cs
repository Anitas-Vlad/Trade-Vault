using TradeVault.Models;

namespace TradeVault.Interfaces;

public interface IBinanceCandleFetcher
{
    Task<List<BinanceKline>> FetchCandlesAsync(string symbol, string interval, int limit);
}