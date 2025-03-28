using Microsoft.EntityFrameworkCore;
using TradeVault.Interfaces;
using TradeVault.Models;

namespace TradeVault.Context.Repositories;

public class CoinsRepository : ICoinsRepository
{
    private readonly TradeVaultContext _context;

    public CoinsRepository(TradeVaultContext context)
    {
        _context = context;
    }

    public async Task<Coin> GetCoinBySymbol(string coinSymbol)
        => await _context.Coins
            .Include(coin => coin.Ema1min)
            .Include(coin => coin.Ema10sec)
            .Include(coin => coin.Candles10Sec)
            .Include(coin => coin.Candles1Min)
            .FirstAsync(coin => coin.Symbol == coinSymbol);
}