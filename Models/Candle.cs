namespace TradeVault.Models;

public class Candle
{
    public int Id { get; set; }
    // public decimal StartPrice { get; set; }//TODO Next Step
    // public decimal EndPrice { get; set; }
    // public decimal MinPrice { get; set; }
    // public decimal MaxPrice { get; set; }
    public decimal AveragePrice { get; set; }
    public DateTime Time { get; set; }
}