using UnityEngine;

[CreateAssetMenu(fileName = "Application Settings Data", menuName = "Application Settings/Application Settings Scriptable Objects/Application Settings")]
public class ApplicationSettings_SO : ScriptableObject
{
    [SerializeField] private SleepTimeoutType sleepTimeout;
    [SerializeField] private int customSleepTime;

    [SerializeField] private bool enableTargetFrameRate = false;
    [SerializeField] private int targetFrameRate = 60;

    public int SleepTimeout => GetSleepTime();
    public bool EnableTargetFrameRate => enableTargetFrameRate;
    public int TargetFrameRate => targetFrameRate;

    private int GetSleepTime()
    {
        if(sleepTimeout == SleepTimeoutType.CustomSleepTime)
        {
            return customSleepTime;
        }

        return (int)sleepTimeout;
    }

    private enum SleepTimeoutType
    {
        CustomSleepTime = -3,
        SystemSettings = UnityEngine.SleepTimeout.SystemSetting,
        NeverSleep = UnityEngine.SleepTimeout.NeverSleep,
    }
}


