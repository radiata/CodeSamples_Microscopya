public static class HintState
{
    private static bool hintsEnabledState = false;
    public static bool HintsEnabledState => hintsEnabledState;

    public delegate void HintStateChangeEvent(bool enabledState);
    public static event HintStateChangeEvent OnHintStateChanged;

    public static void ChangeState(bool enabledState)
    {
        if (enabledState == hintsEnabledState)
        {
            return;
        }

        hintsEnabledState = enabledState;
        OnHintStateChanged?.Invoke(hintsEnabledState);
    }
}
