using TradeVault.Interfaces;
using TradeVault.Models;
using TradeVault.Responses;

namespace TradeVault.Services.BinanceTracking;

public class BinanceCandleTracker : IBinanceCandleTracker
{
    private readonly IBinanceCandleProcessorFactory _binanceCandleProcessorFactory;
    private readonly IMessageValidator _messageValidator;
    
    private readonly List<BinanceCandleProcessor> _processors = new();

    public BinanceCandleTracker(IBinanceCandleProcessorFactory factory,IMessageValidator messageValidator)
    {
        _binanceCandleProcessorFactory = factory;
        _messageValidator = messageValidator;
    }
    
    public void AddProcessor(string symbol, string timeSpan)
    {
        var processor = _binanceCandleProcessorFactory.Create(symbol, timeSpan);
        _processors.Add(processor);
    }
    
    public async Task<BinanceCandleProcessorInfo> AddAndStartCandleProcessorAsync(string message)
    {
        _messageValidator.TryParseTrackingMessageV2(message, out var symbol, out var timeSpan);
        
        var processor = _binanceCandleProcessorFactory.Create(symbol, timeSpan);
        _processors.Add(processor);
        
        Task.Run(() => processor.StartProcessingAsync()); //TODO check if deleting "await" is needed.
        
        return processor.GetInfo();
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