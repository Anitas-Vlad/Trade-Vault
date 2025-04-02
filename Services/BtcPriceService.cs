using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TradeVault.Context;
using TradeVault.Interfaces;
using TradeVault.Responses;

namespace TradeVault.Services;

public class BtcPriceService : IBtcPriceService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly TradeVaultContext _context;

    public BtcPriceService(HttpClient httpClient, IConfiguration configuration, TradeVaultContext context)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _context = context;
    }

    public async Task<decimal> GetBitcoinPriceAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(_configuration["BinanceApiUrls:BTCEUR"]);
            var priceData = JsonSerializer.Deserialize<PriceResponse>(response);

            return decimal.Parse(priceData.price);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching Bitcoin price: {ex.Message}");
            return 0;
        }
    }
}