using UnityEngine;

public class InputModule_Character : I_InputModule
{
    public bool BlockPointer() => true;

    private PointerInteractable_Base heldInteractable;

    private float? lastPinchDistance;
    private float? currentPinchDistance;

    private PointerInteractable_Base[] itemStack;

    private bool lockedNavigation = false;

    public delegate void CharacterModuleInputEvent(PointerInteractable_Base[] itemStack);
    public static event CharacterModuleInputEvent OnCharacterModuleInput;

    public InputModule_Character(bool lockedNavigation)
    {
        this.lockedNavigation = lockedNavigation;
    }

    public bool OnClick(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        bool passedThrough = false;
        bool navObjectsChecked = false;

        for (int i = 0; i < itemStack.Length; i++)
        {
            if (itemStack[i].IsClickable())
            {
                if (passedThrough == true)
                {
                    if (itemStack[i].ReceivePassThrough() == false)
                    {
                        continue;
                    }
                }

                if (lockedNavigation == false
                    || itemStack[i] is not NavigationObject_PointerInteractable)
                {
                    bool terminatePassThrough;

                    if (itemStack[i] is NavigationObject_PointerInteractable)
                    {
                        if (navObjectsChecked == false)
                        {
                            terminatePassThrough = itemStack[i].Click(worldPosition, cameraForward);
                            navObjectsChecked = true;
                        }
                        else
                        {
                            terminatePassThrough = false;
                        }
                    }
                    else
                    {
                        terminatePassThrough = itemStack[i].Click(worldPosition, cameraForward);
                    }

                    if (itemStack[i].SendPassThrough() == false
                        || terminatePassThrough == true)
                    {
                        return true;
                    }
                    else
                    {
                        passedThrough = true;
                    }
                }
            }
        }

        return false;
    }

    public bool OnHoldStart(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        bool passedThrough = false;

        for (int i = 0; i < itemStack.Length; i++)
        {
            if (itemStack[i].IsHoldable())
            {
                if (passedThrough == true)
                {
                    if (itemStack[i].ReceivePassThrough() == false)
                    {
                        continue;
                    }
                }

                if (lockedNavigation == false
                    || itemStack[i] is not NavigationObject_PointerInteractable)
                {
                    bool terminatePassThrough;
                    heldInteractable = itemStack[i].HoldStart(worldPosition, out terminatePassThrough);


                    if (itemStack[i].SendPassThrough() == false
                        || terminatePassThrough == true)
                    {
                        return true;
                    }
                    else
                    {
                        passedThrough = true;
                    }
                }
            }
        }
        return false;
    }

    public bool WhileHolding(Vector3 worldPosition, Vector3 cameraForward)
    {
        if (heldInteractable == null)
        {
            return false;
        }

        heldInteractable.Holding(worldPosition, cameraForward);

        return true;
    }

    public bool OnHoldEnd(Vector3 worldPosition, Vector3 cameraForward)
    {
        if (heldInteractable == null)
        {
            return false;
        }

        heldInteractable.HoldEnd(worldPosition);
        heldInteractable = null;

        return true;
    }

    public bool OnPinchStart(Vector3 positionTouch1, Vector3 positionTouch2)
    {
        lastPinchDistance = Vector2.Distance(positionTouch1, positionTouch2);

        return true;
    }

    public bool WhilePinching(Vector3 positionTouch1, Vector3 positionTouch2)
    {
        currentPinchDistance = Vector2.Distance(positionTouch1, positionTouch2);
        CameraZoomController.ChangeZoomBy((lastPinchDistance.Value - currentPinchDistance.Value) * CameraManager.Instance.ZoomStepStrength);

        lastPinchDistance = currentPinchDistance.Value;
        return true;
    }

    public bool OnPinchEnd(Vector3 positionTouch1, Vector3 positionTouch2)
    {
        WhilePinching(positionTouch1, positionTouch2);

        lastPinchDistance = null;
        currentPinchDistance = null;
        return true;
    }

    public bool OnSwipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        bool passedThrough = false;

        for (int i = 0; i < itemStack.Length; i++)
        {
            if (itemStack[i].IsSwipeable())
            {
                if (passedThrough == true)
                {
                    if (itemStack[i].ReceivePassThrough() == false)
                    {
                        continue;
                    }
                }

                if (lockedNavigation == false
                    || itemStack[i] is not NavigationObject_PointerInteractable)
                {
                    bool terminatePassThrough = itemStack[i].Swipe(worldStartPosition, worldEndPosition, cameraForward);

                    if (itemStack[i].SendPassThrough() == false
                        || terminatePassThrough == true)
                    {
                        return true;
                    }
                    else
                    {
                        passedThrough = true;
                    }
                }
            }
        }

        return false;
    }

    public bool OnContactStart(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        itemStack = PointerInteractable_Base.GetItemStack(worldPosition, cameraForward, targetLayers);
        bool passedThrough = false;

        for (int i = 0; i < itemStack.Length; i++)
        {
            if (itemStack[i].IsPointerContactStartable() == true)
            {

                if (passedThrough == true)
                {
                    if (itemStack[i].ReceivePassThrough() == false)
                    {
                        continue;
                    }
                }

                bool terminatePassThrough;
                terminatePassThrough = itemStack[i].PointerContactStart(worldPosition, cameraForward);

                if (itemStack[i].SendPassThrough() == false
                    || terminatePassThrough == true)
                {
                    break;
                }
                else
                {
                    passedThrough = true;
                }
            }
        }

        OnCharacterModuleInput?.Invoke(itemStack);
        return true;
    }

    public bool OnZoom(float zoomDelta)
    {
        CameraZoomController.ChangeZoomBy(zoomDelta * CameraManager.Instance.ZoomStepStrength);

        return true;
    }
}
