﻿using TradeVault.Models.Helpers;

namespace TradeVault.Models;

    //TODO This is CoinDb (from DataBase), may need in project an AppCoin that holds current things too.
public class Coin
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    
    public List<Candle> Candles10Sec { get; set; }
    public List<Candle> Candles1Min { get; set; }
    
    public List<decimal> Ema10sec { get; set; }
    public List<decimal> Ema1min { get; set; }
}