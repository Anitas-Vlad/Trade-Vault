namespace TradeVault.Interfaces;

public interface INotificationsService
{
    Task SendCurrentPriceAlert(decimal currentPrice);

    Task SendBuyTargetAlert(decimal currentPrice);

    Task SendSellWarningAlert(decimal currentPrice);

    Task SendSellTargetAlert(decimal currentPrice);
    Task SendLowHighAlertRise(decimal currentPrice, string symbol);
    Task SendLowHighAlertDrop(decimal currentPrice, string symbol);
}