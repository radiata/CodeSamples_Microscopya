using UnityEngine;

[CreateAssetMenu(fileName = "Input Manager Settings", menuName = "Input Manager/Input Manager Scriptable Objects/Input Manager Settings")]
public class InputHandler_Settings_SO : ScriptableObject
{
    [Range(0.0f, 1.0f)]
    [SerializeField] private float multiTouchDelay = .2f;

    [Range(0.01f, .5f)]
    [SerializeField] private float swipeMinimumDistance = .05f;

    private InputModule_Shared sharedInputModule = new InputModule_Shared();
    private InputModule_Menu menuInputModule = new InputModule_Menu();
    private InputModule_Character characterInputModule = new InputModule_Character(lockedNavigation: false);
    private InputModule_Character characterLockedNavigationInputModule = new InputModule_Character(lockedNavigation: true);
    private InputModule_GenericVehicle genericVehicleInputModule = new InputModule_GenericVehicle();
    private InputModule_MotorProteinVehicle motorProteinVehicleInputModule = new InputModule_MotorProteinVehicle();
    private InputModule_Character researchModInputModule = new InputModule_Character(lockedNavigation: true);

    public delegate void SettingsUpdated();
    public static event SettingsUpdated OnSettingsUpdated;

    public float GetMultiTouchDelay() => multiTouchDelay;
    public float GetSwipeMinimumDistance() => swipeMinimumDistance;
    public I_InputModule GetSharedInputModule() => sharedInputModule;
    public I_InputModule GetMenuInputModule() => menuInputModule;
    public I_InputModule GetCharacterInputModule() => characterInputModule;
    public I_InputModule GetCharacterLockedNavigationInputModule() => characterLockedNavigationInputModule;
    public I_InputModule GetGenericVehicleInputModule() => genericVehicleInputModule;
    public I_InputModule GetMotorProteinVehicleInputModule() => motorProteinVehicleInputModule;
    public I_InputModule GetResearchModeInputModule() => researchModInputModule;

    [ContextMenu("Update Settings")]
    public void UpdateSettingsEvent()
    {
        OnSettingsUpdated?.Invoke();
    }
}
