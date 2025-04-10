using System.Text.Json;
using TradeVault.Interfaces;
using TradeVault.Models;

namespace TradeVault.Services;

// public class BinanceCandleFetcher : IBinanceCandleFetcher
// {
    // private readonly HttpClient _httpClient;
    //
    // public BinanceCandleFetcher(HttpClient httpClient)
    // {
    //     _httpClient = httpClient;
    // }
    //
    // public async Task<List<BinanceKline>> FetchCandlesAsync(string symbol, string interval, int limit = 200)
    // {
    //     var url = $"https://api.binance.com/api/v3/klines?symbol={symbol.ToUpper()}&interval={interval}&limit={limit}";
    //     var response = await _httpClient.GetAsync(url);
    //     response.EnsureSuccessStatusCode();
    //
    //     var json = await response.Content.ReadAsStringAsync();
    //     var rawData = JsonSerializer.Deserialize<List<List<object>>>(json)!;
    //
    //     return rawData.Select(entry => new BinanceKline
    //         {
    //             OpenTime = Convert.ToInt64(entry[0]),
    //             Open = Convert.ToDecimal(entry[1]),
    //             High = Convert.ToDecimal(entry[2]),
    //             Low = Convert.ToDecimal(entry[3]),
    //             Close = Convert.ToDecimal(entry[4]),
    //             Volume = Convert.ToDecimal(entry[5]),
    //             CloseTime = Convert.ToInt64(entry[6]),
    //             QuoteAssetVolume = Convert.ToDecimal(entry[7]),
    //             NumberOfTrades = Convert.ToInt32(entry[8]),
    //             TakerBuyBaseAssetVolume = Convert.ToDecimal(entry[9]),
    //             TakerBuyQuoteAssetVolume = Convert.ToDecimal(entry[10]),
    //             Ignore = entry[11]
    //         })
    //         .ToList();
    // }
// }