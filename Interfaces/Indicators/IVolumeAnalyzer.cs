using TradeVault.Responses;
using TradeVault.Services.Algorithm.Results;

namespace TradeVault.Interfaces.Indicators;

public interface IVolumeAnalyzer
{
    VolumeAnalysisResult Analyze(List<BinanceKlineResponse> candles);
}