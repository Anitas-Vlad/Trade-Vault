using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TradeVault.Context;
using TradeVault.Interfaces;
using TradeVault.Responses;

namespace TradeVault.Services;

public class BinanceService : IBinanceService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    // private readonly TradeVaultContext _context;

    public BinanceService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        // _context = context;
    }

    public async Task<decimal> GetCurrentPriceFromMessageAsync(string message) //TODO Refactor
    {
        try
        {
            var currencySymbol = ExtractCurrencySymbol(message);
            var response = await GetCurrencyResponse(currencySymbol);

            var priceData = JsonSerializer.Deserialize<PriceResponse>(response);

            return decimal.Parse(priceData.price);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching Bitcoin price: {ex.Message}");
            return 0;
        }
    }

    public async Task<decimal> GetCurrentPriceForSymbol(string symbol)
    {
        try
        {
            var response = await GetCurrencyResponse(symbol);

            var priceData = JsonSerializer.Deserialize<PriceResponse>(response);

            return decimal.Parse(priceData.price);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching Bitcoin price: {ex.Message}");
            return 0;
        }
    }

    private static string ExtractCurrencySymbol(string message) 
        => message.Replace("current ", "");

    private async Task<string> GetCurrencyResponse(string currencyCode) =>
        currencyCode switch
        {
            "btc" => await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:BTCEUR"]),
            "eth" => await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:ETHEUR"]),
            "bnb" => await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:BNBEUR"]),
            "xrp" => await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:XRPEUR"]),
            "ada" => await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:ADAEUR"]),
            "sol" => await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:SOLEUR"]),
            "dot" => await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:DOTEUR"]),
            "matic" => await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:MATICEUR"]),
            "doge" => await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:DOGEEUR"]),
            "ltc" => await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:LTCEUR"]),
            _ => ""
        };
    
    public async Task<List<BinanceKlineResponse>> FetchCandlesAsync(string symbol, string interval, int limit) //TODO Check if it is by default 150, as in IBinanceService interface.
    {
        var url = $"https://api.binance.com/api/v3/klines?symbol={symbol.ToUpper()}EUR&interval={interval}&limit={limit}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
    
        var json = await response.Content.ReadAsStringAsync();
        
        var rawData = JsonSerializer.Deserialize<List<List<JsonElement>>>(json);
        if (rawData == null)
            throw new Exception("Failed to deserialize kline data.");

        var candles = new List<BinanceKlineResponse>();
        
        foreach (var entry in rawData)
        {
            if (entry.Count < 12)
                continue; // Skip malformed entries

            candles.Add(new BinanceKlineResponse
            {
                OpenTime = entry[0].GetInt64(),
                Open = decimal.Parse(entry[1].GetString()!),
                High = decimal.Parse(entry[2].GetString()!),
                Low = decimal.Parse(entry[3].GetString()!),
                Close = decimal.Parse(entry[4].GetString()!),
                Volume = decimal.Parse(entry[5].GetString()!),
                CloseTime = entry[6].GetInt64(),
                QuoteAssetVolume = decimal.Parse(entry[7].GetString()!),
                NumberOfTrades = entry[8].GetInt32(),
                TakerBuyBaseAssetVolume = decimal.Parse(entry[9].GetString()!),
                TakerBuyQuoteAssetVolume = decimal.Parse(entry[10].GetString()!),
                Ignore = entry[11] // still fine as-is
            });
        }

        // Console.WriteLine(candles.Count + " candles found.");
        return candles.OrderBy(candle => candle.CloseTime).ToList();
    }
    
    public async Task<BinanceKlineResponse> FetchLastCandleAsync(string symbol, string interval)
    {
        var url = $"https://api.binance.com/api/v3/klines?symbol={symbol.ToUpper()}EUR&interval={interval}&limit=1";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var rawData = JsonSerializer.Deserialize<List<List<JsonElement>>>(json);

        if (rawData == null || rawData.Count == 0)
            throw new Exception("No data returned from Binance.");
        
        var entry = rawData.First();

        var candle = new BinanceKlineResponse()
        {
            OpenTime = entry[0].GetInt64(),
            Open = decimal.Parse(entry[1].GetString()!),
            High = decimal.Parse(entry[2].GetString()!),
            Low = decimal.Parse(entry[3].GetString()!),
            Close = decimal.Parse(entry[4].GetString()!),
            Volume = decimal.Parse(entry[5].GetString()!),
            CloseTime = entry[6].GetInt64(),
            QuoteAssetVolume = decimal.Parse(entry[7].GetString()!),
            NumberOfTrades = entry[8].GetInt32(),
            TakerBuyBaseAssetVolume = decimal.Parse(entry[9].GetString()!),
            TakerBuyQuoteAssetVolume = decimal.Parse(entry[10].GetString()!),
            Ignore = entry[11] // still fine as-is
        };
        
        // Console.WriteLine($"Last {symbol} candle: " + candle.CloseTime + $"for timeSpan: {interval}");
        
        return candle;
    }
}