using Microsoft.EntityFrameworkCore;
using TradeVault.Interfaces;
using TradeVault.Models;

namespace TradeVault.Context.Repositories;

public class CandlesRepository : ICandlesRepository
{
    private readonly TradeVaultContext _context;

    public CandlesRepository(TradeVaultContext context)
    {
        _context = context;
    }

    public async Task AddCandle(Candle candle)
    {
        var candlesFromDb = await GetCandlesForSecondsTimeSpan(candle.Symbol, candle.TimeSpan);
        if (candlesFromDb.Count >= 200)
        {
            var firstCandle = candlesFromDb.First();
            _context.Candles.Remove(firstCandle);
        }
        _context.Candles.Add(candle);
    }

    public async Task ClearCandles()
    {
        foreach (var candle in _context.Candles)
            _context.Remove(candle);

        await _context.SaveChangesAsync();
    }

    public async Task<List<Candle>> GetCandlesForSecondsTimeSpan(string symbol,int timespan)
    {
        return await _context.Candles
            .Where(candle => candle.Symbol == symbol)
            .Where(candle => candle.TimeSpan == timespan)
            .OrderBy(candle => candle.Time)
            .ToListAsync();
    }
}