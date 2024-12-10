using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    private static List<GraphicRaycaster> graphicRaycasterList;

    private UserControls inputActions;

    private MenuTrackerManager menuTrackerManager;

    private InputModes currentGameInputMode = InputModes.Uninitialized;
    private InputModes previousGameInputMode = InputModes.Uninitialized;


    private InputModes inputMode = InputModes.Uninitialized;
    private PointerHandler pointerHandler;

    private InputHandler_Settings_SO inputManagerSettings;

    private I_InputModule sharedModule = null;
    private I_InputModule activeModule = null;

    private EventSystem eventSystem;
    private PointerEventData pointerEventData;
    private Camera raycastCamera;

    private Coroutine holdingCoroutine;
    private Coroutine pinchingCoroutine;

    private LayerMask activeLayerMask;

    public delegate void OnPointerContactEvent(Vector2 pointerScreenPosition);
    public static OnPointerContactEvent OnPointerContactOccurred;

    public Camera RaycastCamera => raycastCamera;

    public void SetInputManagerSettings(InputHandler_Settings_SO newSettings)
    {
        inputManagerSettings = newSettings;
        OnSettingsUpdate();
    }

    public void ChangeInputMode(InputModes newInputMode, bool forceRefresh = false)
    {
        if (newInputMode == inputMode && forceRefresh == false)
        {
            return;
        }

        CacheInputModes(newInputMode);

        UnsubscribeAll();
        SubscribePointerActions();

        CancelCurrentInputs();

        SubscribeInputModeActions(newInputMode);
        SetActiveInputModule();
        SetActiveLayerMask();
    }

    private void CacheInputModes(InputModes newInputMode)
    {
        if (newInputMode == InputModes.Menu
            || currentGameInputMode == newInputMode)
        {
            return;
        }

        previousGameInputMode = currentGameInputMode;
        currentGameInputMode = newInputMode;
    }

    private void SubscribeInputModeActions(InputModes newInputMode)
    {
        switch (newInputMode)
        {
            case InputModes.Uninitialized:
                break;
            case InputModes.Menu:
                SubscribeMenuActions();
                break;
            case InputModes.Character:
                SubscribeCharacterActions();
                break;
            case InputModes.Character_LockedNavigation:
                SubscribeCharacterLockedNavigationActions();
                break;
            case InputModes.Vehicle_Generic:
                SubscribeGenericVehicleActions();
                break;
            case InputModes.Vehicle_MotorProtein:
                SubscribeMotorProteinVehicleActions();
                break;
            case InputModes.ResearchMode:
                SubscribeResearchActions();
                break;
        }
    }

    private void SetActiveLayerMask()
    {
        switch (inputMode)
        {
            case InputModes.Character:
                activeLayerMask = PointerInteractable_References.PointerDetectionLayers;
                break;
            case InputModes.Character_LockedNavigation:
                activeLayerMask = PointerInteractable_References.PointerDetectionLayers;
                break;
            case InputModes.ResearchMode:
                activeLayerMask = PointerInteractable_References.ResearchMode_PointerDetectionLayers;
                break;
            default:
                DebugWrapper.Log("Active Layer Mask was not set/changed.", gameObject);
                break;
        }
    }

    private void CancelCurrentInputs()
    {
        if (holdingCoroutine != null)
        {
            OnHoldEnd(inputActions.Generic.PointerContactPosition.ReadValue<Vector2>());
        }
        if (pinchingCoroutine != null)
        {
            pointerHandler.EndAllPointerContacts_EnhancedTouch();
        }
    }

    #region Event Subscriptions
    private void UnsubscribeAll()
    {
        UnsubscribePointerActions();
        UnsubscribeMenuActions();
        UnsubscribeCharacterActions();
        UnsubscribeGenericVehicleActions();
        UnsubscribeCharacterLockedNavigationActions();
        UnsubscribeMotorProteinVehicleActions();
        UnsubscribeResearchActions();
    }

    private void SubscribePointerActions()
    {
        inputActions.Generic.PointerContact.performed += PointerContactAction;
        inputActions.Generic.PointerContact.canceled += PointerContactAction;

        Touch.onFingerDown += PointerContactAction_EnhancedTouch;
        Touch.onFingerUp += PointerContactAction_EnhancedTouch;
    }

    private void UnsubscribePointerActions()
    {
        inputActions.Generic.PointerContact.performed -= PointerContactAction;
        inputActions.Generic.PointerContact.canceled -= PointerContactAction;

        Touch.onFingerDown -= PointerContactAction_EnhancedTouch;
        Touch.onFingerUp -= PointerContactAction_EnhancedTouch;
    }

    private void SubscribeMenuActions()
    {
        inputMode = InputModes.Menu;
    }

    private void UnsubscribeMenuActions()
    {

    }

    private void SubscribeCharacterActions()
    {
        inputActions.Generic.Zoom.performed += OnZoom;

        inputMode = InputModes.Character;
    }

    private void UnsubscribeCharacterActions()
    {
        inputActions.Generic.Zoom.performed -= OnZoom;
    }

    private void SubscribeCharacterLockedNavigationActions()
    {
        inputActions.Generic.Zoom.performed += OnZoom;

        inputMode = InputModes.Character_LockedNavigation;
    }

    private void UnsubscribeCharacterLockedNavigationActions()
    {
        inputActions.Generic.Zoom.performed -= OnZoom;
    }

    private void SubscribeGenericVehicleActions()
    {
        throw new System.NotImplementedException();
    }

    private void UnsubscribeGenericVehicleActions()
    {

    }

    private void SubscribeMotorProteinVehicleActions()
    {
        inputActions.Generic.Zoom.performed += OnZoom;

        inputMode = InputModes.Vehicle_MotorProtein;
    }

    private void UnsubscribeMotorProteinVehicleActions()
    {
        inputActions.Generic.Zoom.performed -= OnZoom;
    }

    private void SubscribeResearchActions()
    {
        inputActions.Generic.Zoom.performed += OnZoom;

        inputMode = InputModes.ResearchMode;
    }

    private void UnsubscribeResearchActions()
    {
        inputActions.Generic.Zoom.performed -= OnZoom;
    }
    #endregion

    public void OnPinchStart_EnhancedTouch(Touch primaryTouch, Touch secondaryTouch)
    {
        Vector2 positionTouch1 = raycastCamera.ScreenToViewportPoint(primaryTouch.screenPosition);
        Vector2 positionTouch2 = raycastCamera.ScreenToViewportPoint(secondaryTouch.screenPosition);

        pinchingCoroutine = StartCoroutine(Pinching_EnhancedTouch(primaryTouch.finger, secondaryTouch.finger));

        bool consumed = activeModule.OnPinchStart(positionTouch1, positionTouch2);

        if (consumed == false)
        {
            sharedModule.OnPinchStart(positionTouch1, positionTouch2);
        }

    }

    public void WhilePinching_EnhancedTouch(Touch primaryTouch, Touch secondaryTouch)
    {
        Vector2 positionTouch1 = raycastCamera.ScreenToViewportPoint(primaryTouch.screenPosition);
        Vector2 positionTouch2 = raycastCamera.ScreenToViewportPoint(secondaryTouch.screenPosition);

        bool consumed = activeModule.WhilePinching(positionTouch1, positionTouch2);

        if (consumed == false)
        {
            sharedModule.WhilePinching(positionTouch1, positionTouch2);
        }
    }

    public void OnPinchEnd_EnhancedTouch(Touch primaryTouch, Touch secondaryTouch)
    {
        if (pinchingCoroutine == null)
        {
            return;
        }

        Vector2 positionTouch1 = raycastCamera.ScreenToViewportPoint(primaryTouch.screenPosition);
        Vector2 positionTouch2 = raycastCamera.ScreenToViewportPoint(secondaryTouch.screenPosition);

        if (pinchingCoroutine != null)
        {
            StopCoroutine(pinchingCoroutine);
            pinchingCoroutine = null;
        }

        bool consumed = activeModule.OnPinchEnd(positionTouch1, positionTouch2);

        if (consumed == false)
        {
            sharedModule.OnPinchEnd(positionTouch1, positionTouch2);
        }
    }

    public void OnSwipe(Vector2 startPosition, Vector2 endPosition)
    {
        Vector3 worldStartPosition = raycastCamera.ScreenToWorldPoint(startPosition);
        Vector3 worldEndPosition = raycastCamera.ScreenToWorldPoint(endPosition);

        bool consumed = activeModule.OnSwipe(worldStartPosition, worldEndPosition, raycastCamera.transform.forward, activeLayerMask);

        if (consumed == false)
        {
            sharedModule.OnSwipe(worldStartPosition, worldEndPosition, raycastCamera.transform.forward, activeLayerMask);
        }
    }

    public void OnClick(Vector2 pointerScreenPosition)
    {
        var worldPosition = raycastCamera.ScreenToWorldPoint(pointerScreenPosition);

        bool consumed = activeModule.OnClick(worldPosition, raycastCamera.transform.forward, activeLayerMask);

        if (consumed == false)
        {
            sharedModule.OnClick(worldPosition, raycastCamera.transform.forward, activeLayerMask);
        }
    }

    public void OnHoldStart(InputAction pointerPositionInputAction)
    {
        Vector3 worldPosition = raycastCamera.ScreenToWorldPoint(pointerPositionInputAction.ReadValue<Vector2>());

        holdingCoroutine = StartCoroutine(Holding(pointerPositionInputAction));

        bool consumed = activeModule.OnHoldStart(worldPosition, raycastCamera.transform.forward, activeLayerMask);

        if (consumed == false)
        {
            sharedModule.OnHoldStart(worldPosition, raycastCamera.transform.forward, activeLayerMask);
        }
    }

    public void OnHoldStart_EnhancedTouch(Finger finger)
    {
        Vector3 worldPosition = raycastCamera.ScreenToWorldPoint(finger.screenPosition);

        holdingCoroutine = StartCoroutine(Holding_EnhancedTouch(finger));

        bool consumed = activeModule.OnHoldStart(worldPosition, raycastCamera.transform.forward, activeLayerMask);

        if (consumed == false)
        {
            sharedModule.OnHoldStart(worldPosition, raycastCamera.transform.forward, activeLayerMask);
        }
    }

    public void WhileHolding(Vector2 pointerScreenPosition)
    {
        Vector3 worldPosition = raycastCamera.ScreenToWorldPoint(pointerScreenPosition);

        bool consumed = activeModule.WhileHolding(worldPosition, raycastCamera.transform.forward);

        if (consumed == false)
        {
            sharedModule.WhileHolding(worldPosition, raycastCamera.transform.forward);
        }
    }

    public void OnHoldEnd(Vector2 pointerScreenPosition)
    {
        Vector3 worldPosition = raycastCamera.ScreenToWorldPoint(pointerScreenPosition);

        if (holdingCoroutine != null)
        {
            StopCoroutine(holdingCoroutine);
            holdingCoroutine = null;
        }

        bool consumed = activeModule.OnHoldEnd(worldPosition, raycastCamera.transform.forward);

        if (consumed == false)
        {
            sharedModule.OnHoldEnd(worldPosition, raycastCamera.transform.forward);
        }
    }

    public void ResetHold(Vector3 worldPosition, bool limitToNavigationLayers = true)
    {
        if (limitToNavigationLayers)
        {
            activeModule.OnContactStart(worldPosition, raycastCamera.transform.forward, PointerInteractable_References.CharacterNavigationLayers);
            activeModule.OnHoldStart(worldPosition, raycastCamera.transform.forward, PointerInteractable_References.CharacterNavigationLayers);
        }
        else
        {
            activeModule.OnContactStart(worldPosition, raycastCamera.transform.forward, activeLayerMask);
            activeModule.OnHoldStart(worldPosition, raycastCamera.transform.forward, activeLayerMask);
        }
    }

    public void OnPointerContactStart(Vector2 pointerScreenPosition)
    {
        OnPointerContactOccurred?.Invoke(pointerScreenPosition);

        var worldPosition = raycastCamera.ScreenToWorldPoint(pointerScreenPosition);

        bool consumed = activeModule.OnContactStart(worldPosition, raycastCamera.transform.forward, activeLayerMask);

        if (consumed == false)
        {
            sharedModule.OnContactStart(worldPosition, raycastCamera.transform.forward, activeLayerMask);
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        bool consumed = activeModule.OnZoom(context.ReadValue<Vector2>().y);

        if (consumed == false)
        {
            sharedModule.OnZoom(context.ReadValue<Vector2>().y);
        }
    }

    private void PointerContactAction(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Disabled:
                break;
            case InputActionPhase.Waiting:
                break;
            case InputActionPhase.Started:
                break;
            case InputActionPhase.Performed:
                if (BlockPointerInput(inputActions.Generic.PointerContactPosition.ReadValue<Vector2>()) == false)
                {
                    pointerHandler.OnPointerContactStart(inputActions.Generic.PointerContactPosition);
                }
                break;
            case InputActionPhase.Canceled:
                pointerHandler.OnPointerContactEnd();
                break;
        }
    }

    public void PointerContactAction_EnhancedTouch(Finger finger)
    {
        if (finger.currentTouch.ended == true)
        {
            pointerHandler.EndPointerContact_EnhancedTouch(finger);
            return;
        }

        if (finger.currentTouch.phase == TouchPhase.Began)
        {
            if (BlockPointerInput(finger.screenPosition) == true)
            {
                return;
            }

            pointerHandler.StartPointerContact_EnhancedTouch(finger);
        }
    }

    private bool BlockPointerInput(Vector2 pointerScreenPosition)
    {
        if (activeModule == null)
        {
            return true;
        }
        if (activeModule.BlockPointer() == false)
        {
            return false;
        }

        pointerEventData.position = pointerScreenPosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        List<GraphicRaycaster> staleGraphicRaycasters = new List<GraphicRaycaster>();

        foreach (GraphicRaycaster graphicRaycaster in graphicRaycasterList)
        {
            if (graphicRaycaster != null)
            {
                graphicRaycaster.Raycast(pointerEventData, raycastResults);
            }
            else
            {
                staleGraphicRaycasters.Add(graphicRaycaster);
            }
        }

        if (staleGraphicRaycasters.Count > 0)
        {
            GraphicRaycasterRegistry.UnregisterGraphicRaycaster(staleGraphicRaycasters.ToArray());
        }

        return raycastResults.Count > 0;
    }

    private void SetActiveInputModule()
    {
        switch (inputMode)
        {
            case InputModes.Uninitialized:
                activeModule = null;
                return;
            case InputModes.Menu:
                activeModule = inputManagerSettings.GetMenuInputModule();
                return;
            case InputModes.Character:
                activeModule = inputManagerSettings.GetCharacterInputModule();
                return;
            case InputModes.Character_LockedNavigation:
                activeModule = inputManagerSettings.GetCharacterLockedNavigationInputModule();
                return;
            case InputModes.Vehicle_Generic:
                activeModule = inputManagerSettings.GetGenericVehicleInputModule();
                return;
            case InputModes.Vehicle_MotorProtein:
                activeModule = inputManagerSettings.GetMotorProteinVehicleInputModule();
                return;
            case InputModes.ResearchMode:
                activeModule = inputManagerSettings.GetResearchModeInputModule();
                return;
            default:
                DebugWrapper.LogWarning("Unhandled Input Mode - InputHandler.SetActiveInputModule()", gameObject);
                break;
        }

        activeModule = null;
        return;
    }

    private void OnSettingsUpdate()
    {
        if (inputManagerSettings == null)
        {
            return;
        }

        var swipeMinimumDistance = inputManagerSettings.GetSwipeMinimumDistance();
        var multiTouchDelay = inputManagerSettings.GetMultiTouchDelay();

        pointerHandler.Initialize(this, multiTouchDelay, swipeMinimumDistance);

        sharedModule = inputManagerSettings.GetSharedInputModule();
        SetActiveInputModule();
    }

    private void OnCameraUpdate()
    {
        if (CameraManager.Instance == null)
        {
            return;
        }

        raycastCamera = CameraManager.Instance.ActiveCamera;
    }

    private void OnGraphicRaycasterRegistryUpdate()
    {
        graphicRaycasterList = GraphicRaycasterRegistry.GraphicRaycasterList;
    }

    private void OnMenuStateChange(BaseMenuTracker _, bool isOpen)
    {
        if (menuTrackerManager.IsAnyMenuOpen == true)
        {
            ChangeInputMode(InputModes.Menu);
        }
        else if (currentGameInputMode != InputModes.Uninitialized)
        {
            ChangeInputMode(currentGameInputMode);
        }
    }

    private void OnResearchModeStateChange(bool isEnabled)
    {
        if (isEnabled == true)
        {
            ChangeInputMode(InputModes.ResearchMode);
        }
        else if (previousGameInputMode != InputModes.Uninitialized)
        {
            ChangeInputMode(previousGameInputMode);
        }
    }

    private void Awake()
    {
        Instance = this;

        inputActions = new UserControls();

        if (graphicRaycasterList == null)
        {
            graphicRaycasterList = new List<GraphicRaycaster>();
        }

        if (pointerHandler == null)
        {
            pointerHandler = gameObject.AddComponent<PointerHandler>();
        }

        if (eventSystem == null)
        {
            eventSystem = gameObject.AddComponent<EventSystem>();
            gameObject.AddComponent<InputSystemUIInputModule>();
        }

        if (menuTrackerManager == null)
        {
            menuTrackerManager = gameObject.AddComponent<MenuTrackerManager>();
        }
    }

    private void OnEnable()
    {
        OnSettingsUpdate();
        InputHandler_Settings_SO.OnSettingsUpdated += OnSettingsUpdate;

        OnCameraUpdate();
        CameraManager.OnCameraChanged += OnCameraUpdate;

        OnGraphicRaycasterRegistryUpdate();
        GraphicRaycasterRegistry.OnGraphicRaycasterRegistryUpdate += OnGraphicRaycasterRegistryUpdate;

        BaseMenuTracker.OnMenuStateChange += OnMenuStateChange;
        ResearchModeState.OnResearchModeStateChanged += OnResearchModeStateChange;

        ChangeInputMode(inputMode, true);
        inputActions.Enable();
        EnhancedTouchSupport.Enable();
        pointerEventData = new PointerEventData(eventSystem);
    }

    private void OnDisable()
    {
        InputHandler_Settings_SO.OnSettingsUpdated -= OnSettingsUpdate;
        CameraManager.OnCameraChanged -= OnCameraUpdate;
        GraphicRaycasterRegistry.OnGraphicRaycasterRegistryUpdate -= OnGraphicRaycasterRegistryUpdate;
        BaseMenuTracker.OnMenuStateChange -= OnMenuStateChange;
        ResearchModeState.OnResearchModeStateChanged -= OnResearchModeStateChange;
        EnhancedTouchSupport.Disable();

        UnsubscribeAll();
        inputActions.Disable();
        pointerEventData = null;
    }

    private IEnumerator Holding(InputAction pointerPositionInputAction)
    {
        while (true)
        {
            yield return null;
            WhileHolding(pointerPositionInputAction.ReadValue<Vector2>());
        }
    }

    private IEnumerator Holding_EnhancedTouch(Finger finger)
    {
        while (true)
        {
            yield return null;
            WhileHolding(finger.screenPosition);
        }
    }

    private IEnumerator Pinching_EnhancedTouch(Finger fingerOne, Finger fingerTwo)
    {
        while (true)
        {
            yield return null;
            WhilePinching_EnhancedTouch(fingerOne.currentTouch, fingerTwo.currentTouch);
        }
    }

    #region Debug Context Menu Commands
    [ContextMenu("Set to Debug Character Input Mode")]
    private void DebugCharacterInputMode()
    {
        ChangeInputMode(InputModes.Character);
    }
    #endregion
}