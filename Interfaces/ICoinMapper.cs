using TradeVault.Models;

namespace TradeVault.Interfaces;

public interface ICoinMapper
{
    CoinStats Map(Coin coin);
}