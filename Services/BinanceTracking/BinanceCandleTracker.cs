using TradeVault.Interfaces;
using TradeVault.Models;
using TradeVault.Responses;

namespace TradeVault.Services.BinanceTracking;

public class BinanceCandleTracker : IBinanceCandleTracker
{
    private readonly IBinanceCandleProcessorFactory _binanceCandleProcessorFactory;
    private readonly IInputValidator _inputValidator;
    
    private readonly List<BinanceCandleProcessor> _processors = new();

    public BinanceCandleTracker(IBinanceCandleProcessorFactory factory,IInputValidator inputValidator)
    {
        _binanceCandleProcessorFactory = factory;
        _inputValidator = inputValidator;
    }
    
    public void AddProcessor(string symbol, string timeSpan)
    {
        var processor = _binanceCandleProcessorFactory.Create(symbol, timeSpan);
        _processors.Add(processor);
    }
    
    public async Task AddAndStartCandleProcessorAsync(string message)
    {
        _inputValidator.TryParseTrackingMessageV2(message, out var symbol, out var timeSpan);
        
        var processor = _binanceCandleProcessorFactory.Create(symbol, timeSpan);
        _processors.Add(processor);
        
        Task.Run(() => processor.StartProcessingAsync());
    }

    public async Task StartAllAsync()
    {
        var tasks = _processors.Select(tracker => tracker.StartProcessingAsync()).ToList();
        await Task.WhenAll(tasks);
    }

    public void StopAll()
    {
        foreach (var processor in _processors)
        {
            processor.StopProcessing();
        }
    }
}