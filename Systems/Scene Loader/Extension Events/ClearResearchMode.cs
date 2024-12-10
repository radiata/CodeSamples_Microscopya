public static class ClearResearchMode
{
    public delegate void ClearResearchModeEvent();
    public static ClearResearchModeEvent OnClearResearchMode;

    public static void ExecuteClearResearchMode()
    {
        OnClearResearchMode?.Invoke();
    }
}
