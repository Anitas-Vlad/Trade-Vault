namespace TradeVault.Services.Algorithm.Results;

public class VolumeAnalysisResult
{
    public decimal AverageVolume { get; set; }
    public decimal LatestVolume { get; set; }
    public bool IsVolumeIncreasing { get; set; }
}