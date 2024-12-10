using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PointerHandler : MonoBehaviour
{
    private InputHandler inputManager;

    private float touchDelay;
    private float touchTime = 0f;
    private float swipeDelta;

    private Coroutine pointerDelayRoutine = null;

    private InputAction pointerContactPositionAction = null;

    private bool receiveInput = true;
    private Finger primaryContactFinger;
    private Finger secondaryContactFinger;

    private Vector2 pointerStartPosition = Vector2.zero;

    private bool pointerContactOccurred = false;
    private bool secondaryTouchOccurred = false;

    public void OnPointerContactStart(InputAction pointerPosition)
    {
        if (secondaryTouchOccurred == true)
        {
            return;
        }

        pointerContactOccurred = true;

        touchTime = 0;

        pointerContactPositionAction = pointerPosition;

        pointerStartPosition = pointerPosition.ReadValue<Vector2>();

        if (pointerDelayRoutine != null)
        {
            StopCoroutine(pointerDelayRoutine);
            pointerDelayRoutine = null;
        }

        inputManager.OnPointerContactStart(pointerStartPosition);
        pointerDelayRoutine = StartCoroutine(PointerDelayCheck(pointerPosition));
    }

    public void OnPointerContactStart_EnhancedTouch(Finger finger)
    {
        if (secondaryTouchOccurred == true)
        {
            return;
        }

        primaryContactFinger = finger;
        pointerContactOccurred = true;

        touchTime = 0;

        pointerStartPosition = finger.screenPosition;

        if (pointerDelayRoutine != null)
        {
            StopCoroutine(pointerDelayRoutine);
            pointerDelayRoutine = null;
        }

        inputManager.OnPointerContactStart(pointerStartPosition);
        pointerDelayRoutine = StartCoroutine(PointerDelayCheck_EnhancedTouch(finger));
    }


    public void OnPointerContactEnd()
    {
        if (pointerContactOccurred == false)
        {
            return;
        }

        Vector2 pointerEndPosition = pointerContactPositionAction.ReadValue<Vector2>();

        if (pointerDelayRoutine != null)
        {
            StopCoroutine(pointerDelayRoutine);
            pointerDelayRoutine = null;
        }

        pointerContactOccurred = false;
        pointerContactPositionAction = null;

        if (touchTime <= touchDelay)
        {
            Camera camera = inputManager.RaycastCamera;

            if (Vector2.Distance(camera.ScreenToViewportPoint(pointerStartPosition), camera.ScreenToViewportPoint(pointerEndPosition)) > swipeDelta)
            {
                inputManager.OnSwipe(pointerStartPosition, pointerEndPosition);
            }
            else
            {
                inputManager.OnClick(pointerStartPosition);
            }
        }
        else
        {
            inputManager.OnHoldEnd(pointerEndPosition);
        }
    }

    public void PointerContactEnd_EnhancedTouch(Touch primaryTouch)
    {

        if (pointerContactOccurred == false)
        {
            return;
        }

        Vector2 pointerEndPosition = primaryTouch.screenPosition;

        if (pointerDelayRoutine != null)
        {
            StopCoroutine(pointerDelayRoutine);
            pointerDelayRoutine = null;
        }

        pointerContactOccurred = false;
        pointerContactPositionAction = null;

        if (secondaryTouchOccurred == true)
        {
            if (secondaryContactFinger != null)
            {
                inputManager.OnPinchEnd_EnhancedTouch(primaryTouch, secondaryContactFinger.currentTouch);
            }
            return;
        }

        if (touchTime <= touchDelay)
        {
            Camera camera = inputManager.RaycastCamera;

            if (Vector2.Distance(camera.ScreenToViewportPoint(pointerStartPosition), camera.ScreenToViewportPoint(pointerEndPosition)) > swipeDelta)
            {
                inputManager.OnSwipe(pointerStartPosition, pointerEndPosition);
            }
            else
            {
                inputManager.OnClick(pointerStartPosition);
            }
        }
        else
        {
            inputManager.OnHoldEnd(pointerEndPosition);
        }

        primaryContactFinger = null;
    }

    public void OnSecondaryContactStart_EnhancedTouch(Finger primaryFinger, Finger secondaryFinger)
    {
        if (pointerContactOccurred == false)
        {
            return;
        }

        if (touchTime > touchDelay)
        {
            return;
        }

        receiveInput = false;

        secondaryContactFinger = secondaryFinger;
        secondaryTouchOccurred = true;

        if (pointerDelayRoutine != null)
        {
            StopCoroutine(pointerDelayRoutine);
            pointerDelayRoutine = null;
        }

        inputManager.OnPinchStart_EnhancedTouch(primaryFinger.currentTouch, secondaryFinger.currentTouch);
    }

    public void OnSecondaryContactEnd_EnhancedTouch(Touch primaryTouch, Touch secondaryTouch)
    {

        inputManager.OnPinchEnd_EnhancedTouch(primaryTouch, secondaryTouch);
        secondaryContactFinger = null;
    }

    public void StartPointerContact_EnhancedTouch(Finger finger)
    {
        if (receiveInput == false)
        {
            return;
        }

        if (primaryContactFinger == null)
        {
            OnPointerContactStart_EnhancedTouch(finger);
            return;
        }

        if (secondaryContactFinger == null)
        {
            OnSecondaryContactStart_EnhancedTouch(primaryContactFinger, finger);
            return;
        }
    }

    public void EndPointerContact_EnhancedTouch(Finger finger)
    {
        if (finger == secondaryContactFinger)
        {
            OnSecondaryContactEnd_EnhancedTouch(primaryContactFinger.currentTouch, secondaryContactFinger.currentTouch);
        }

        if (finger == primaryContactFinger)
        {
            PointerContactEnd_EnhancedTouch(primaryContactFinger.currentTouch);
        }

        if (ResetFlag() == true)
        {
            ResetPointerInput();
        }
    }

    public void EndAllPointerContacts_EnhancedTouch()
    {
        if (secondaryContactFinger != null)
        {
            OnSecondaryContactEnd_EnhancedTouch(primaryContactFinger.currentTouch, secondaryContactFinger.currentTouch);
        }

        if (primaryContactFinger != null)
        {
            PointerContactEnd_EnhancedTouch(primaryContactFinger.currentTouch);
        }

        ResetPointerInput();
    }

    private bool ResetFlag()
    {
        foreach (Touch touch in Touch.activeTouches)
        {
            if (touch.ended == false)
            {
                return false;
            }
        }

        return true;
    }

    private void ResetPointerInput()
    {
        touchTime = 0;
        pointerContactPositionAction = null;
        pointerStartPosition = Vector2.zero;

        primaryContactFinger = null;
        secondaryContactFinger = null;

        pointerContactOccurred = false;
        secondaryTouchOccurred = false;

        if (pointerDelayRoutine != null)
        {
            StopCoroutine(pointerDelayRoutine);
            pointerDelayRoutine = null;
        }

        receiveInput = true;
    }

    public void Initialize(InputHandler newInputManager, float newTouchDelay, float newSwipeDelta)
    {
        inputManager = newInputManager;
        touchDelay = newTouchDelay;
        swipeDelta = newSwipeDelta;
    }

    private IEnumerator PointerDelayCheck(InputAction pointerPosition)
    {
        while (touchTime <= touchDelay)
        {
            yield return null;
            touchTime += Time.deltaTime;
        }

        inputManager.OnHoldStart(pointerPosition);
    }

    private IEnumerator PointerDelayCheck_EnhancedTouch(Finger finger)
    {
        while (touchTime <= touchDelay)
        {
            yield return null;
            touchTime += Time.deltaTime;
        }

        receiveInput = false;
        inputManager.OnHoldStart_EnhancedTouch(finger);
    }

    private void Awake()
    {
        InputHandlerEvents.OnSkipMultiTouchDelay += OnSkipMultiTouchDelay;
    }

    private void OnDestroy()
    {
        InputHandlerEvents.OnSkipMultiTouchDelay -= OnSkipMultiTouchDelay;
    }

    private void OnSkipMultiTouchDelay()
    {
        touchTime = touchDelay + 1;
    }
}
