namespace TradeVault.Models;

public class Coin
{
    public int Id { get; set; }
    public string Symbol { get; set; }

    public List<Candle> Candles { get; set; }
}