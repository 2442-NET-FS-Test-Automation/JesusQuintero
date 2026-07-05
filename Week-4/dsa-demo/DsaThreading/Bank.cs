namespace DsaThreating;

public class Bank
{
    public long Balance; // mutable state

    private readonly object _gate = new();

    public void DepositUnsafe(long amount) => Balance += amount; // read-modifiy-write: NOT-ATOMIC

    public void DepositSafe(long amount)
    {
        lock (_gate) // only one thread can enter this code block at time
        {
            Balance += amount;
        }
    }
}