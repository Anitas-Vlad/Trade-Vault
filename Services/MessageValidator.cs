using TradeVault.Interfaces;

namespace TradeVault.Services;

public class MessageValidator : IMessageValidator
{
    private static readonly HashSet<string> ValidCurrencies = new()
    {
        "btc", "eth", "bnb", "xrp", "ada", "sol", "dot", "matic", "doge", "ltc"
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
}