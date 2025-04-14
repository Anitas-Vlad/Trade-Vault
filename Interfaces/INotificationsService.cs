namespace TradeVault.Interfaces;

public interface INotificationsService
{
    Task SendCurrentPriceAlert(decimal currentPrice);

    Task SendBuyTargetAlert(decimal currentPrice);

    Task SendSellWarningAlert(decimal currentPrice);

    Task SendSellTargetAlert(decimal currentPrice);
}