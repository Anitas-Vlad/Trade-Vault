using TradeVault.Interfaces;
using TradeVault.Interfaces.Indicators;
using TradeVault.Models;
using TradeVault.Models.Enums;
using TradeVault.Responses;

namespace TradeVault.Services.BinanceTracking;

public class BinanceCandleProcessor : IBinanceCandleProcessor
{
    private readonly IBinanceService _binanceService;
    private readonly ITelegramService _telegramService;
    private readonly IAlgorithmService _algorithmService;
    private readonly IMacdSignalDetector _macdSignalDetector;
    private readonly ITradingSignalDetectorService _tradingSignalDetectorService;
    
    private readonly CancellationTokenSource _cts = new();

    private readonly string _symbol;
    private readonly string _timeSpan;
    private long _lastCandleCloseTime;
    private TradeSignal _tradeSignal = TradeSignal.Default;
    public TradeSignal LastMacdMessageType = TradeSignal.Default;
    public List<BinanceKlineResponse> Candles { get; set; } = new();

    public BinanceCandleProcessor(IBinanceService binanceService, ITelegramService telegramService,
        IAlgorithmService algorithmService, IMacdSignalDetector macdSignalDetector, ITradingSignalDetectorService tradingSignalDetectorService, string symbol, string timeSpan)
    {
        _binanceService = binanceService;
        _telegramService = telegramService;
        _algorithmService = algorithmService;
        _macdSignalDetector = macdSignalDetector;
        _tradingSignalDetectorService = tradingSignalDetectorService;
        _symbol = symbol;
        _timeSpan = timeSpan;
    }

    public BinanceCandleProcessorInfo GetInfo()
        => new(_symbol, _timeSpan, _lastCandleCloseTime, _tradeSignal);

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
                AddNewCandle(lastBinanceCandle);

                try
                {
                    // Method 1
                    // var candlesCloseValues = Candles.Select(candle => candle.Close).ToList();
                    // _tradeSignal = _algorithmService.CheckMacdSignal(Candles, 6, 13, 9, _symbol);
                    //
                    // _macdResponseType = _macdSignalDetector.CheckMacdSignal(Candles, 6, 13, 9);
                    //
                    // Console.WriteLine("MacdResponseType: " + _macdResponseType);
                    
                    // METHOD 2
                    
                    _tradeSignal = await _tradingSignalDetectorService.GetTradeSignalAsync(_symbol, _timeSpan);
                    // Console.WriteLine($"MacdResponseType: {_tradeSignal} + time:{_timeSpan}");
                    HandleMacdResponse();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in CheckMacdSignal: {ex.Message}");
                    _tradeSignal = TradeSignal.Default;
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }
    }

    private void AddNewCandle(BinanceKlineResponse lastBinanceCandle)
    {
        Candles.RemoveAt(0);
        Candles.Add(lastBinanceCandle);
        _lastCandleCloseTime = lastBinanceCandle.CloseTime;
    }

    private void HandleMacdResponse()
    {
        switch (_tradeSignal)
        {
            case TradeSignal.Buy:
                _tradeSignal = TradeSignal.Default;
                LastMacdMessageType = TradeSignal.Buy;
                _telegramService.SendMessageAsync($"\ud83d\udfe9\ud83d\udfe9\ud83d\udfe9{_symbol} : Buy signal \n" +
                                                  $"TimeSpan: {_timeSpan} \n" +
                                                  $"(MACD crossed below Signal Line)");
                break;

            case TradeSignal.Sell:
                _tradeSignal = TradeSignal.Default;
                LastMacdMessageType = TradeSignal.Sell;
                _telegramService.SendMessageAsync($"\ud83d\udfe5\ud83d\udfe5\ud83d\udfe5{_symbol} : Sell signal \n" +
                                                  $"TimeSpan: {_timeSpan} \n" +
                                                  $"(MACD crossed above Signal Line)");
                break;

            case TradeSignal.Default:
                break;
        }
    }

    public void StopProcessing()
    {
        _telegramService.SendMessageAsync($"Macd stopped for {_symbol}, Time: {_timeSpan}");
        _cts.Cancel();
    }
}