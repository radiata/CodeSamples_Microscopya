using System.Threading;

public static class Task_Utilities
{
    public static void RefreshToken(ref CancellationTokenSource tokenSource)
    {
        tokenSource?.Cancel();
        tokenSource?.Dispose();
        tokenSource = new CancellationTokenSource();
    }
}
