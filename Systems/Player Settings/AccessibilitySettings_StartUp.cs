using UnityEngine;

public class AccessibilitySettings_StartUp : MonoBehaviour
{
    private void Awake()
    {
        CameraTiltControl_AccessibilitySetting.InitializeSetting();
    }
}
