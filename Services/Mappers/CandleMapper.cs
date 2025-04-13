using TradeVault.Interfaces;
using TradeVault.Models;
using TradeVault.Responses;

namespace TradeVault.Services.Mappers;

public class CandleMapper : ICandleMapper
{
    public Candle Map(BinanceKlineResponse kline)
    {
        return new Candle
        {
            
        };
    }
}