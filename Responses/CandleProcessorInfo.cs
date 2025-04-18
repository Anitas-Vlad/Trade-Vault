using TradeVault.Models.Enums;

namespace TradeVault.Responses;

public class CandleProcessorInfo
{
    public string Symbol { get; set; }
    public int SecondsTimeSpan { get; set; }
    public TradeSignal TradeSignal { get; set; }

    public CandleProcessorInfo(string symbol, int secondsTimeSpan, TradeSignal tradeSignal)
    {
        Symbol = symbol;
        SecondsTimeSpan = secondsTimeSpan;
        TradeSignal = tradeSignal;
    }
}