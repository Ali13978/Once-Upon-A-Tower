namespace AtomicMarketApiClient
{
    public enum State : ushort
    {
        Waiting = 0,
        Listed = 1,
        Cancelled = 2,
        Sold = 3,
        Invalid = 4
    }
}