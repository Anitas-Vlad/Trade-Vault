using TradeVault.Models;

namespace TradeVault.Interfaces;

public interface ITrackingService
{
    Task TrackCoin(Coin coin);
}