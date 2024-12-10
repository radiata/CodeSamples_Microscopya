public static class ResearchModeState
{
    private static bool researchModeEnabledState = false;
    public static bool ResearchModeEnabledState => researchModeEnabledState;

    public delegate void ResearchModeStateChangeEvent(bool enabledState);
    public static event ResearchModeStateChangeEvent OnResearchModeStateChanged;

    public static void ChangeState(bool enabledState)
    {
        if(enabledState == researchModeEnabledState)
        {
            return;
        }

        researchModeEnabledState = enabledState;
        OnResearchModeStateChanged?.Invoke(enabledState);
    }

    private static void OnClearResearchMode()
    {
        ChangeState(false);
    }

    static ResearchModeState()
    {
        ClearResearchMode.OnClearResearchMode += OnClearResearchMode;
    }
}
