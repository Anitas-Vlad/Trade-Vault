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

    public async Task AddCandle(Candle entity) // TODO keep a maximum of 200 of each candle.
    {
        var candlesFromDb = await GetCandlesForSecondsTimeSpan(entity.TimeSpan);
        if (candlesFromDb.Count >= 200)
        {
            var firstCandle = candlesFromDb.First();
            _context.Candles.Remove(firstCandle);
        }
        _context.Candles.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task ClearCandles()
    {
        foreach (var candle in _context.Candles)
            _context.Remove(candle);

        await _context.SaveChangesAsync();
    }

    public async Task<List<Candle>> GetCandlesForSecondsTimeSpan(int timespan)
    {
        return await _context.Candles
            .Where(candle => candle.TimeSpan == timespan)
            .OrderBy(candle => candle.Time)
            .ToListAsync();
    }
}