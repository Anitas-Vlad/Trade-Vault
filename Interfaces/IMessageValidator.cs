namespace TradeVault.Interfaces;

public interface IMessageValidator
{
    void TryParseTrackingMessage(string message, out string symbol, out int timeSpan);
}