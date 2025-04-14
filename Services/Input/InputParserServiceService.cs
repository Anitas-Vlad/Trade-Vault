using TelegramBitcoinPrices.Input;
using TradeVault.Interfaces;

namespace TradeVault.Services.Input;

public class InputParserService : IInputParserService //TODO Check
{
    // public BtcRange ParseBtcRange(string input)
    // {
    //     InputValidator.ValidateLowHighCommand(input, out var );
    //
    //     var parts = input.Split(' ');
    //     var lowBtcPrice = decimal.Parse(parts[1]);
    //     var highBtcPrice = decimal.Parse(parts[2]);
    //
    //     if (lowBtcPrice >= highBtcPrice)
    //         throw new ArgumentException("The first number must be lower than the second number.");
    //
    //     return new BtcRange
    //     {
    //         LowBtcPrice = lowBtcPrice,
    //         HighBtcPrice = highBtcPrice
    //     };
    // }
}