namespace TradeVault.Responses;

public class CandleProcessorInfo
{
    public string Symbol { get; set; }
    public int SecondsTimeSpan { get; set; }

    public CandleProcessorInfo(string symbol, int secondsTimeSpan)
    {
        Symbol = symbol;
        SecondsTimeSpan = secondsTimeSpan;
    }
}