using TradeVault.Interfaces;

namespace TradeVault.Services;

public class CandleTracker : ICandleTracker
{
    private readonly ICandleProcessorFactory _candleProcessorFactory;
    private readonly List<CandleProcessor> _processors = new();
    

    public CandleTracker(ICandleProcessorFactory candleProcessorFactory)
    {
        _candleProcessorFactory = candleProcessorFactory;
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
    
    public async Task AddAndStartProcessorAsync(string symbol, int intervalSeconds)
    {
        var processor = _candleProcessorFactory.Create(symbol, intervalSeconds);
        _processors.Add(processor);
        await processor.StartProcessingAsync();
    }
}