using TradeVault.Interfaces;
using TradeVault.Models;

namespace TradeVault.Services.Mappers;

public class CoinMapper : ICoinMapper
{
    public CoinStats Map(Coin coin) =>
        new()
        {
            Symbol = coin.Symbol,
            Candles1Min = coin.Candles1Min,
            Candles10Sec = coin.Candles10Sec,
            
            Ema1min = coin.Ema1min,
            Ema10sec = coin.Ema10sec,

            CurrentPrice = 0
        };
}