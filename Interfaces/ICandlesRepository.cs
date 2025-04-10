using TradeVault.Models;

namespace TradeVault.Interfaces;

public interface ICandlesRepository
{
    Task AddCandle(Candle candle);
    Task ClearCandles();
    Task<List<Candle>> GetCandlesForSecondsTimeSpan(string symbol, int timespan);
}