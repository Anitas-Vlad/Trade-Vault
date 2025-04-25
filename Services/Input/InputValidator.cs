using TelegramBitcoinPrices.Input;
using TradeVault.Interfaces;

namespace TradeVault.Services.Input;

public class InputValidator : IInputValidator
{
    private readonly ITelegramService _telegramService;
    private static readonly HashSet<string> ValidCurrencies = new()
    {
        "btc", "eth", "bnb", "xrp", "ada", "sol", "dot", "matic", "doge", "ltc"
    };

    private static readonly HashSet<string> ValidBinanceTimeSpans = new()
    {
        "1m", "3m", "5m", "15m", "30m",
        "1h", "2h", "4h", "6h", "8h", "12h",
        "1d", "3d", "1w", "1M"
    };

    public InputValidator(ITelegramService telegramService)
    {
        _telegramService = telegramService;
    }

    public void TryParseTrackingMessage(string message, out string symbol, out int timeSpan)
    {
        symbol = string.Empty;
        timeSpan = 0;

        var parts = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3)
            _telegramService.SendMessageAsync("Invalid message.");

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
        ValidateSymbol(symbol);

        timeSpan = parts[2].ToLower();
        if (!ValidBinanceTimeSpans.Contains(timeSpan))
            throw new ArgumentException("Unknown time-span.");
    }

    public static void ValidateLowHighCommand(string input, out string symbol, out decimal lowPrice,
        out decimal highPrice)
    {
        var parts = input.Split(' ');

        if (parts.Length != 4 || parts[0].ToLower() != "lh")
            throw new ArgumentException("Invalid input format. Expected format: 'start <lowNumber> <highNumber>'.");

        symbol = parts[1].ToLower();

        var isSymbolValid = ValidateSymbol(symbol);
        if (!isSymbolValid)
            throw new ArgumentException("Unknown symbol.");

        if (!decimal.TryParse(parts[2], out _))
            throw new ArgumentException("The first number must be a valid decimal.");

        if (!decimal.TryParse(parts[3], out _))
            throw new ArgumentException("The second number must be a valid decimal.");

        lowPrice = decimal.Parse(parts[2]);
        highPrice = decimal.Parse(parts[3]);

        if (lowPrice >= highPrice)
            throw new ArgumentException("First price must be higher than the high price.");
    }

    public static bool ValidateSymbol(string symbol)
        => ValidCurrencies.Contains(symbol);
}