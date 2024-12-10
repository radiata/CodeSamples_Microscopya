public class CameraTiltControl_AccessibilitySetting : I_AccessibilitySetting
{
    public static bool tiltDisabled = false;

    public delegate void CameraTiltControlStateChange(bool disabledState);
    public static event CameraTiltControlStateChange OnCameraTiltControlStateChanged;

    public static void InitializeSetting()
    {
        tiltDisabled = PlayerPrefs_Utilities.GetCameraTiltAccessibilitySetting();
    }

    public static void ChangeTiltState(bool newState)
    {
        tiltDisabled = newState;
        PlayerPrefs_Utilities.SetCameraTiltAccessibilitySetting(tiltDisabled);
        OnCameraTiltControlStateChanged?.Invoke(tiltDisabled);
    }
}
