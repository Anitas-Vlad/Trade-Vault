using TradeVault.Interfaces;
using TradeVault.Models;
using TradeVault.Models.Enums;
using TradeVault.Responses;

namespace TradeVault.Services.BinanceTracking;

public class BinanceCandleProcessor : IBinanceCandleProcessor
{
    private readonly IBinanceService _binanceService;
    private readonly ITelegramService _telegramService;
    private readonly IAlgorithmService _algorithmService;

    private readonly CancellationTokenSource _cts = new();

    private readonly string _symbol;
    private readonly string _timeSpan;
    private long _lastCandleCloseTime;
    private MacdResponseType _macdResponseType = MacdResponseType.Default;
    public MacdResponseType LastMacdMessageType = MacdResponseType.Default;
    public List<BinanceKlineResponse> Candles { get; set; } = new();

    public BinanceCandleProcessor(IBinanceService binanceService, ITelegramService telegramService,
        IAlgorithmService algorithmService, string symbol, string timeSpan)
    {
        _binanceService = binanceService;
        _telegramService = telegramService;
        _algorithmService = algorithmService;
        _symbol = symbol;
        _timeSpan = timeSpan;
    }

    public BinanceCandleProcessorInfo GetInfo()
        => new(_symbol, _timeSpan, _lastCandleCloseTime, _macdResponseType);

    public async Task StartProcessingAsync() //TODO (ShortPeriod, LongPeriod, SignalPeriod)
    {
        var token = _cts.Token;
        Candles = await _binanceService.FetchCandlesAsync(_symbol, _timeSpan);
        _lastCandleCloseTime = Candles.Last().CloseTime;
        
        await _telegramService.SendMessageAsync(
            $"Tracking Binance {_symbol}: {_timeSpan} candles.");

        while (!token.IsCancellationRequested)
        {
            var lastBinanceCandle = await _binanceService.FetchLastCandleAsync(_symbol, _timeSpan);
            if (lastBinanceCandle.CloseTime != _lastCandleCloseTime)
            {
                Candles.RemoveAt(0);
                Candles.Add(lastBinanceCandle);
                _lastCandleCloseTime = lastBinanceCandle.CloseTime;

                try
                {
                    var candlesCloseValues = Candles.Select(candle => candle.Close).ToList();

                    _macdResponseType = _algorithmService.CheckMacdSignal(candlesCloseValues, 6, 13, 9, _symbol);
                    Console.WriteLine("MacdResponseType: " + _macdResponseType);
                    HandleMacdResponse();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in CheckMacdSignal: {ex.Message}");
                    _macdResponseType = MacdResponseType.Default;
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }
    }

    private void HandleMacdResponse()
    {
        switch (_macdResponseType)
        {
            case MacdResponseType.Buy:
                _macdResponseType = MacdResponseType.Default;
                LastMacdMessageType = MacdResponseType.Buy;
                _telegramService.SendMessageAsync($"\ud83d\udfe9\ud83d\udfe9\ud83d\udfe9{_symbol} : Buy signal \n" +
                                                  $"TimeSpan: {_timeSpan} \n" +
                                                  $"(MACD crossed below Signal Line)");
                break;

            case MacdResponseType.Sell:
                _macdResponseType = MacdResponseType.Default;
                LastMacdMessageType = MacdResponseType.Sell;
                _telegramService.SendMessageAsync($"\ud83d\udfe5\ud83d\udfe5\ud83d\udfe5{_symbol} : Sell signal \n" +
                                                  $"TimeSpan: {_timeSpan} \n" +
                                                  $"(MACD crossed above Signal Line)");
                break;

            case MacdResponseType.Default:
                break;
        }
    }

    public void StopProcessing()
    {
        _telegramService.SendMessageAsync($"Macd stopped for {_symbol}, Time: {_timeSpan}");
        _cts.Cancel();
    }
}