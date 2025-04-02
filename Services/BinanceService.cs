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
    private readonly TradeVaultContext _context;

    public BinanceService(HttpClient httpClient, IConfiguration configuration, TradeVaultContext context)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _context = context;
    }

    public async Task<decimal> GetCurrencyPriceAsync(string message)
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
}