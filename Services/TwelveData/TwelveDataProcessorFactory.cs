using Microsoft.Extensions.Configuration;
using TradeVault.Interfaces.TwelveData;

namespace TradeVault.Services.TwelveData;

public class TwelveDataProcessorFactory : ITwelveDataProcessorFactory
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public TwelveDataProcessorFactory(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public TwelveDataProcessor Create(string symbol, string shortPeriod, string longPeriod, string signalPeriod) =>
        new(_httpClient, _configuration, symbol, shortPeriod, longPeriod, signalPeriod);
}