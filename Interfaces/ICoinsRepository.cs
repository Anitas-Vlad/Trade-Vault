using TradeVault.Models;

namespace TradeVault.Interfaces;

public interface ICoinsRepository
{
    Task<Coin> GetCoinBySymbol(string coinSymbol);
}