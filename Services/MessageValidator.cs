using TradeVault.Interfaces;

namespace TradeVault.Services;

public class MessageValidator : IMessageValidator
{
    private static readonly HashSet<string> ValidCurrencies = new()
    {
        "btc", "eth", "bnb", "xrp", "ada", "sol", "dot", "matic", "doge", "ltc"
    };
    
    private static readonly HashSet<string> ValidTimeSpans = new()
    {
        "1m", "3m", "5m", "15m", "30m",
        "1h", "2h", "4h", "6h", "8h", "12h",
        "1d", "3d", "1w", "1M"
    };

    public void TryParseTrackingMessage(string message, out string symbol, out int timeSpan)
    {
        symbol = string.Empty;
        timeSpan = 0;

        var parts = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3)
            throw new ArgumentException("Invalid message.");

        symbol = parts[1].ToLower();
        if (!ValidCurrencies.Contains(symbol))
            throw new ArgumentException("Unknown currency.");

        if (!int.TryParse(parts[2], out timeSpan) || timeSpan <= 0)
            throw new ArgumentException("Invalid TimeSpan.");
    }
    
    public void TryParseTrackingMessageV2(string message, out string symbol, out string timeSpan)
    {
        symbol = string.Empty;
        timeSpan = "";

        var parts = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3)
            throw new ArgumentException("Invalid message.");

        symbol = parts[1].ToLower();
        if (!ValidCurrencies.Contains(symbol))
            throw new ArgumentException("Unknown currency.");

        timeSpan = parts[2].ToLower();
        if (!ValidTimeSpans.Contains(timeSpan))
            throw new ArgumentException("Unknown time-span.");
    }
}