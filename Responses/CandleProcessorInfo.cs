using TradeVault.Models.Enums;

namespace TradeVault.Responses;

public class CandleProcessorInfo
{
    public string Symbol { get; set; }
    public int SecondsTimeSpan { get; set; }
    public MacdResponseType MacdResponseType { get; set; }

    public CandleProcessorInfo(string symbol, int secondsTimeSpan, MacdResponseType macdResponseType)
    {
        Symbol = symbol;
        SecondsTimeSpan = secondsTimeSpan;
        MacdResponseType = macdResponseType;
    }
}