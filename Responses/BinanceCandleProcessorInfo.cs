using TradeVault.Models.Enums;

namespace TradeVault.Responses;

public class BinanceCandleProcessorInfo
{
    public string Symbol { get; set; }
    public string TimeSpan { get; set; }
    public long LastCandleCloseTime { get; set; }
    public MacdResponseType MacdResponseType { get; set; }

    public BinanceCandleProcessorInfo(string symbol, string timeSpan, long lastCandleCloseTime, MacdResponseType macdResponseType)
    {
        Symbol = symbol;
        TimeSpan = timeSpan;
        LastCandleCloseTime = lastCandleCloseTime;
        MacdResponseType = macdResponseType;
    }
}