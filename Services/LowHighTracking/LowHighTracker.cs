using TradeVault.Interfaces;
using TradeVault.Services.Input;

namespace TradeVault.Services.LowHighTracking;

public class LowHighTracker : ILowHighTracker
{
    private readonly ILowHighProcessorFactory _lowHighProcessorFactory;
    private readonly IInputValidator _inputValidator;
    private readonly List<LowHighProcessor> _processors = new();

    public LowHighTracker(ILowHighProcessorFactory lowHighProcessorFactory, IInputValidator inputValidator)
    {
        _lowHighProcessorFactory = lowHighProcessorFactory;
        _inputValidator = inputValidator;
    }

    // public void AddProcessor(string symbol, decimal lowPrice, decimal highPrice)
    // {
    //     var existingProcessor = _processors.FirstOrDefault(p => p.Symbol == symbol);
    //     
    //     if (existingProcessor != null)
    //         existingProcessor.Reset(lowPrice, highPrice);
    //     else
    //     {
    //         var processor = _lowHighProcessorFactory.Create(symbol, lowPrice, highPrice);
    //         _processors.Add(processor);
    //     }
    // }

    public async Task AddLowHighTracker(string message)
    {
        InputValidator.ValidateLowHighCommand(message, out var symbol, out var lowPrice, out var highPrice);

        var existingProcessor = _processors.FirstOrDefault(p => p.Symbol == symbol);

        if (existingProcessor != null)
        {
            existingProcessor.Reset(lowPrice, highPrice);
        }
        else
        {
            var processor = _lowHighProcessorFactory.Create(symbol, lowPrice, highPrice);
            _processors.Add(processor);

            Task.Run(() => processor.StartLowHighNotifications(message));
        }
    }

    public async Task StopLowHighTracker(string message)
    {
        var symbol = message.Split(' ').Last();
        Console.WriteLine($"Stopping low high processor for {symbol}");
        InputValidator.ValidateSymbol(symbol);
        var processor = _processors.FirstOrDefault(p => p.Symbol == symbol);
        if (processor == null)
            throw new ArgumentException("There is no processor with symbol: " + symbol);

        await processor.Stop();
        _processors.Remove(processor);
    }
}