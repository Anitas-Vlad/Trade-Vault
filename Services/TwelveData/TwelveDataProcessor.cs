using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TradeVault.Services.TwelveData.Models;

namespace TradeVault.Services.TwelveData;

public class TwelveDataProcessor
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _symbol;
    private readonly string _shortPeriod;
    private readonly string _longPeriod;
    private readonly string _signalPeriod;

    public TwelveDataProcessor(HttpClient httpClient, IConfiguration configuration, string symbol, string shortPeriod, string longPeriod, string signalPeriod)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _symbol = symbol;
        _shortPeriod = shortPeriod;
        _longPeriod = longPeriod;
        _signalPeriod = signalPeriod;
    }

    public async Task<List<(DateTime Time, string Type)>> GetMacdCrossoversAsync(string symbol, string interval = "5min")
    {
        var apiKey = await _httpClient.GetStringAsync(_configuration["TwelveData"]);

        var url =
            $"https://api.twelvedata.com/macd?symbol={symbol}/EUR&interval={interval}&short_period={_shortPeriod}&long_period={_longPeriod}&signal_period={_signalPeriod}&apikey={apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var macdData = JsonSerializer.Deserialize<MacdResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (macdData?.values == null || macdData.values.Count < 2)
            return new List<(DateTime Time, string Type)>();

        var signals = new List<(DateTime, string)>();

        for (var i = 1; i < macdData.values.Count; i++)
        {
            var prev = macdData.values[i - 1];
            var curr = macdData.values[i];

            var wasBelow = prev.macd < prev.macd_signal;
            var isAbove = curr.macd > curr.macd_signal;

            if (wasBelow && isAbove)
                signals.Add((curr.datetime, "BUY"));

            var wasAbove = prev.macd > prev.macd_signal;
            var isBelow = curr.macd < curr.macd_signal;

            if (wasAbove && isBelow)
                signals.Add((curr.datetime, "SELL"));
        }

        return signals;
    }
}