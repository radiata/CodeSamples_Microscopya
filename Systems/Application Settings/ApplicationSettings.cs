using UnityEngine;

public static class ApplicationSettings
{
    private static int sleepTimeout;
    private static bool enableTargetFrameRate;
    private static int targetFrameRate;

    public static void Initialize(int targetFrameRate, bool enableTargetFrameRate, int sleepTimeout)
    {
        SetVariables(targetFrameRate, enableTargetFrameRate, sleepTimeout);
        UpdateSettings();
    }

    public static void SetVariables(int targetFrameRate, bool enableTargetFrameRate, int sleepTimeout)
    {
        ApplicationSettings.sleepTimeout = sleepTimeout;
        ApplicationSettings.enableTargetFrameRate = enableTargetFrameRate;
        ApplicationSettings.targetFrameRate = targetFrameRate;
    }

    public static void UpdateSettings()
    {
        Screen.sleepTimeout = sleepTimeout;

        if (enableTargetFrameRate == true)
        {
            Application.targetFrameRate =
                targetFrameRate < Screen.currentResolution.refreshRateRatio.value
                ? targetFrameRate : (int)Screen.currentResolution.refreshRateRatio.value;
        }
    }
}
