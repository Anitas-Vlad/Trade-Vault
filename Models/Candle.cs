using System.ComponentModel.DataAnnotations.Schema;

namespace TradeVault.Models;

public class Candle
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    [NotMapped]
    public List<decimal> PriceValues { get; set; }
    public decimal StartPrice { get; set; }
    public decimal EndPrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal AveragePrice { get; set; }
    public DateTime Time { get; set; }
    public int TimeSpan { get; set; }
    
    //TODO Change
    
    // public int Id { get; set; }
    // public string Symbol { get; set; }
    // public long OpenTime { get; set; }
    // public long CloseTime { get; set; }
    // public decimal Open { get; set; }
    // public decimal Close { get; set; }
    // public decimal Low { get; set; }
    // public decimal High { get; set; }
    // public decimal Volume { get; set; }
    // public decimal QuoteAssetVolume { get; set; }
    // public int NumberOfTrades { get; set; }
    // public decimal TakerBuyBaseAssetVolume { get; set; }
    // public decimal TakerBuyQuoteAssetVolume { get; set; }
    // public object Ignore { get; set; } // Unused field returned by Binance
    // public string TimeSpan { get; set; } //  "1m, 3m, 5m, 10m, 1d"

}