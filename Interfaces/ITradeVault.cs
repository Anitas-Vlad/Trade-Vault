namespace TradeVault.Interfaces;

public interface ITradeVault
{
    Task Run();
    Task RefreshDb();
}