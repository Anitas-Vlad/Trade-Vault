using TradeVault.Context;
using TradeVault.Context.Repositories;
using TradeVault.Interfaces;
using TradeVault.Models;

namespace TradeVault.Services;

public class CandleProcessor : ICandleProcessor
{
    private readonly IBinanceService _binanceService;
    private readonly ICandlesRepository _candlesRepository;
    private readonly ICoinsRepository _coinsRepository;
    private readonly TradeVaultContext _context;
    
    private readonly string _symbol;
    private readonly int _secondsTimeSpan;
    private readonly CancellationTokenSource _cts = new();

    public CandleProcessor(TradeVaultContext context, IBinanceService binanceService,ICoinsRepository coinsRepository, ICandlesRepository candlesRepository, string symbol, int secondsTimeSpan)
    {
        _context = context;
        _candlesRepository = candlesRepository;
        _coinsRepository = coinsRepository;
        _binanceService = binanceService;
        
        _symbol = symbol;
        _secondsTimeSpan = secondsTimeSpan;
    }

    public async Task StartProcessingAsync()
    {
        var token = _cts.Token;
        var coin = _coinsRepository.GetCoinBySymbol(_symbol);

        while (!token.IsCancellationRequested)
        {
            var candle = await CreateCandleAsync(token);
            await _candlesRepository.AddCandle(candle);
            (await coin).Candles.Add(candle);
            await _context.SaveChangesAsync(token);
        }
    }

    private async Task<Candle> CreateCandleAsync(CancellationToken token)
    {
        var candle = new Candle
        {
            Symbol = _symbol,
            Time = DateTime.UtcNow,
            PriceValues = new List<decimal>(),
            TimeSpan = _secondsTimeSpan
        };

        for (var i = 0; i < _secondsTimeSpan; i++)
        {
            if (token.IsCancellationRequested)
            {
                UpdateCandleStats(candle);
                return candle;
            }

            var newPrice = await _binanceService.GetCurrencyPriceAsync(_symbol);
            candle.PriceValues.Add(newPrice);
            
            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }
        
        UpdateCandleStats(candle);
        return candle;
    }

    private static void UpdateCandleStats(Candle candle)
    {
        candle.MinPrice = candle.PriceValues.Min();
        candle.MaxPrice = candle.PriceValues.Max();
        candle.StartPrice = candle.PriceValues.First();
        candle.EndPrice = candle.PriceValues.Last();
    }
    
    public void StopProcessing() => _cts.Cancel();
}