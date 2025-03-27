using TradeVault.Models.Helpers;

namespace TradeVault.Models;

public class Coin
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public decimal CurrentPrice { get; set; }
}