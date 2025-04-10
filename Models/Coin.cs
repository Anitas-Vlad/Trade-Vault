namespace TradeVault.Models;

public class Coin
{
    public int Id { get; set; }
    public string Symbol { get; set; }

    public List<Candle> Candles { get; set; }

    public List<decimal> GetPricesHistoryForTimeSpan(int secondsTimeSpan) =>
        Candles
            .Where(candle => candle.TimeSpan == secondsTimeSpan)
            .OrderBy(candle => candle.Time)
            .Select(candle => candle.AveragePrice)
            .ToList();
}