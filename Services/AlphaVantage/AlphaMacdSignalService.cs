using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace TradeVault.Services.AlphaVantage;

public class AlphaMacdSignalService : IAlphaMacdSignalService
{
    private readonly HttpClient _httpClient;

    // private readonly string _apiKey;
    private readonly IConfiguration _configuration;


    public AlphaMacdSignalService(HttpClient httpClient, string apiKey, IConfiguration configuration)
    {
        _httpClient = httpClient;
        // _apiKey = apiKey;
        _configuration = configuration;
    }

    public async Task CheckMacdSignalAsync(string symbol = "BTCEUR", string interval = "5min")
    {
        var apiKey = await _httpClient.GetStringAsync(_configuration["AlphaVantageApiKey"]);
        var url =
            $"https://www.alphavantage.co/query?function=MACD&symbol={symbol}&interval={interval}&series_type=close&apikey={apiKey}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new ArgumentException("Failed to fetch MACD data.");

        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);

        if (!document.RootElement.TryGetProperty("Technical Analysis: MACD", out var macdData))
            throw new ArgumentException("MACD data not found.");

        var entries = macdData.EnumerateObject()
            .OrderByDescending(e => e.Name)
            .Take(2)
            .ToList();

        if (entries.Count < 2) return;

        var latest = entries[0].Value;
        var previous = entries[1].Value;

        var macdCurrent = decimal.Parse(latest.GetProperty("MACD").GetString() ?? "0");
        var signalCurrent = decimal.Parse(latest.GetProperty("MACD_Signal").GetString() ?? "0");

        var macdPrev = decimal.Parse(previous.GetProperty("MACD").GetString() ?? "0");
        var signalPrev = decimal.Parse(previous.GetProperty("MACD_Signal").GetString() ?? "0");

        if (macdPrev < signalPrev && macdCurrent > signalCurrent)
        {
            Console.WriteLine("📈 BUY Signal (MACD crossed above Signal)");
        }
        else if (macdPrev > signalPrev && macdCurrent < signalCurrent)
        {
            Console.WriteLine("📉 SELL Signal (MACD crossed below Signal)");
        }
        else
        {
            Console.WriteLine("⏳ No crossover signal yet.");
        }
    }
}