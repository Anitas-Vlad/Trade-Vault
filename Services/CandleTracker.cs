using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using TradeVault.Interfaces;

namespace TradeVault.Services;

public class CandleTracker : ICandleTracker
{
    private readonly ICandleProcessorFactory _candleProcessorFactory;
    private readonly IMessageValidator _messageValidator;
    private readonly List<CandleProcessor> _processors = new();
    

    public CandleTracker(ICandleProcessorFactory candleProcessorFactory, IMessageValidator messageValidator)
    {
        _candleProcessorFactory = candleProcessorFactory;
        _messageValidator = messageValidator;
    }

    public void AddProcessor(string symbol, int secondsTimeSpan)
    {
        var processor = _candleProcessorFactory.Create(symbol, secondsTimeSpan);
        _processors.Add(processor);
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
    
    public async Task AddAndStartProcessorAsync(string message)
    {
        _messageValidator.TryParseTrackingMessage(message, out var symbol, out var timeSpan);
        
        var processor = _candleProcessorFactory.Create(symbol, timeSpan);
        _processors.Add(processor);
        await processor.StartProcessingAsync();
    }
}