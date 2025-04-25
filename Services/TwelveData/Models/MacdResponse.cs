namespace TradeVault.Services.TwelveData.Models;

public class MacdResponse
{
    public List<MacdValue> values { get; set; } = new();
    public string status { get; set; } = string.Empty;
}