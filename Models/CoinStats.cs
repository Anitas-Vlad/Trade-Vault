using TradeVault.Models.Helpers;

namespace TradeVault.Models;

public class CoinStats
{
    public string Symbol { get; set; }
    public decimal CurrentPrice { get; set; }

    public List<Candle> Candles { get; set; }
    public List<decimal> Ema10sec { get; set; }
    public List<decimal> Ema1min { get; set; }
}