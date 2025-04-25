namespace TradeVault.Services.TwelveData.Models;

public class MacdValue
{
    public DateTime datetime { get; set; }
    public decimal macd { get; set; }
    public decimal macd_signal { get; set; }
    public decimal macd_histogram { get; set; }
}