using TelegramBitcoinPrices.Input;

namespace TradeVault.Interfaces;

public interface IInputValidator //TODO change into Static for performance
{
    void TryParseTrackingMessage(string message, out string symbol, out int timeSpan); //TODO Decide if you'll continue using the continuous ongoing candle
    void TryParseTrackingMessageV2(string message, out string symbol, out string timeSpan);
}