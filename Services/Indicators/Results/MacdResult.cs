﻿namespace TradeVault.Services.Indicators.Results;

public class MacdResult
{
    public List<decimal> MacdLine { get; set; } = new();
    public List<decimal> SignalLine { get; set; } = new();
    public List<decimal> Histogram { get; set; } = new();
}