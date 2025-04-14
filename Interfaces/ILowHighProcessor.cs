namespace TradeVault.Interfaces;

public interface ILowHighProcessor
{
    Task StartLowHighNotifications(string input);
    void Reset(decimal lowPrice, decimal highPrice);
    Task Stop();
}