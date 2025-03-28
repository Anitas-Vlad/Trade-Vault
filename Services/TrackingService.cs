using TradeVault.Interfaces;
using TradeVault.Models;

namespace TradeVault.Services;

public class TrackingService : ITrackingService
{
    public readonly ICoinMapper _coinMapper;
    
    public TrackingService(ICoinMapper coinMapper)
    {
        _coinMapper = coinMapper;
    }

    public async Task TrackCoin(Coin coin)
    {
        var coinStats = _coinMapper.Map(coin);
        
    }
}