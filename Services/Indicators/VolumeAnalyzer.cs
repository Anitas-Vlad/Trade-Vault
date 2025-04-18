using TradeVault.Interfaces.Indicators;
using TradeVault.Responses;
using TradeVault.Services.Algorithm.Results;

namespace TradeVault.Services.Indicators;

public class VolumeAnalyzer : IVolumeAnalyzer
{
    public VolumeAnalysisResult Analyze(List<BinanceKlineResponse> candles)
    {
        var volumes = candles.Select(c => c.Volume).ToList();
        var averageVolume = volumes.Average();
        var latestVolume = volumes.Last();

        return new VolumeAnalysisResult
        {
            AverageVolume = averageVolume,
            LatestVolume = latestVolume,
            IsVolumeIncreasing = latestVolume > averageVolume
        };
    }
}