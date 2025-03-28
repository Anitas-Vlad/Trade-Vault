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
    
    public async Task AddCandle(Candle entity)
    { 
        _context.Candles.Add(entity);
        await _context.SaveChangesAsync();
    }
}