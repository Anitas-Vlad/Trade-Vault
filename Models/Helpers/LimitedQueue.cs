namespace TradeVault.Models.Helpers;

public class LimitedQueue<T> : Queue<T>
{
    public int Limit { get; set; }

    public LimitedQueue(int limit)
    {
        Limit = limit;
    }

    public void Enqueue(T value)
    {
        if (Count >= Limit)
            Dequeue();

        base.Enqueue(value);
    }
}