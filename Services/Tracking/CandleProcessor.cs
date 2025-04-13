using TradeVault.Context;
using TradeVault.Interfaces;
using TradeVault.Models;
using TradeVault.Models.Enums;
using TradeVault.Responses;

namespace TradeVault.Services.Tracking;

public class CandleProcessor : ICandleProcessor
{
    private readonly IBinanceService _binanceService;
    private readonly ITelegramService _telegramService;
    private readonly ICandlesRepository _candlesRepository;
    private readonly ICoinsRepository _coinsRepository;
    private readonly IAlgorithmService _algorithmService;
    private readonly TradeVaultContext _context;

    private readonly CancellationTokenSource _cts = new();

    public List<decimal> pricesHistory = new();
    private readonly string _symbol;
    private readonly int _secondsTimeSpan;
    private MacdResponseType _macdResponseType = MacdResponseType.Default;

    public CandleProcessor(TradeVaultContext context, ITelegramService telegramService,
        IAlgorithmService algorithmService,
        IBinanceService binanceService, ICoinsRepository coinsRepository,
        ICandlesRepository candlesRepository, string symbol, int secondsTimeSpan)
    {
        _context = context;
        _telegramService = telegramService;
        _candlesRepository = candlesRepository;
        _coinsRepository = coinsRepository;
        _binanceService = binanceService;
        _algorithmService = algorithmService;

        _symbol = symbol;
        _secondsTimeSpan = secondsTimeSpan;
    }

    public CandleProcessorInfo GetInfo()
        => new(_symbol, _secondsTimeSpan, _macdResponseType);

    public async Task StartProcessingAsync() //TODO Modify for dynamic Macd Parameters.
    {
        var token = _cts.Token;
        var coinTask = _coinsRepository.GetCoinBySymbol(_symbol);

        while (!token.IsCancellationRequested)
        {
            var candle = await CreateCandleAsync(token);

            await _candlesRepository.AddCandle(candle);
            var coin = await coinTask;

            coin.Candles.Add(candle);

            if (pricesHistory.Count == 0)
                pricesHistory = coin.GetPricesHistoryForTimeSpan(_secondsTimeSpan);
            
            try
            {
                _macdResponseType = _algorithmService.CheckMacdSignal(pricesHistory, 6, 13, 9, _symbol);
                HandleMacdResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckMacdSignal: {ex.Message}");
                _macdResponseType = MacdResponseType.Default;
            }
            
            // HandleMacdResponse();
            await _context.SaveChangesAsync(token);
        }
    }

    private void HandleMacdResponse()
    {
        switch (_macdResponseType)
        {
            case MacdResponseType.Buy:
                _telegramService.SendMessageAsync($"💰{_symbol} Buy signal (MACD crossed below Signal Line)");
                _macdResponseType = MacdResponseType.Default;
                break;
            case MacdResponseType.Sell:
                _telegramService.SendMessageAsync($"📤{_symbol} Sell signal (MACD crossed above Signal Line)");
                _macdResponseType = MacdResponseType.Default;
                break;
            case MacdResponseType.Default:
                break;
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

        candle.AveragePrice = candle.PriceValues.Average();

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