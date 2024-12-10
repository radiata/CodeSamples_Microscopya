using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputHandler_Profiler : InputHandler
{
    ProfilerMarker markerStartETSPinch = new ProfilerMarker("profileMarker_Start Enhanced Touch Pinch");
    ProfilerMarker markerStopETSPinch = new ProfilerMarker("profileMarker_Stop Enhanced Touch Pinch");
    ProfilerMarker markerETSPinching = new ProfilerMarker("profileMarker_Enhanced Touch Pinching Call");
    ProfilerMarker markerETSHolding = new ProfilerMarker("profileMarker_Enhanced Touch Holding Call");
    ProfilerMarker markerStartETSHold = new ProfilerMarker("profileMarker_Start Enhanced Touch Hold");
    ProfilerMarker markerStopETSHold = new ProfilerMarker("profileMarker_Stop Enhanced Touch Hold");
    ProfilerMarker markerOnZoom = new ProfilerMarker("profileMarker_OnZoom call");
    ProfilerMarker markerStartETSPointer = new ProfilerMarker("profileMarker_Start Enhanced Touch Pointer Contact");

    public void OnPinchStart_EnhancedTouch(Touch primaryTouch, Touch secondaryTouch)
    {
        markerStartETSPinch.Begin();
        base.OnPinchStart_EnhancedTouch(primaryTouch, secondaryTouch);
        markerStartETSPinch.End();
    }

    public void WhilePinching_EnhancedTouch(Touch primaryTouch, Touch secondaryTouch)
    {
        markerETSPinching.Begin();
        base.WhilePinching_EnhancedTouch(primaryTouch, secondaryTouch);
        markerETSPinching.End();
    }

    public void OnPinchEnd_EnhancedTouch(Touch primaryTouch, Touch secondaryTouch)
    {
        markerStopETSPinch.Begin();
        OnPinchEnd_EnhancedTouch(primaryTouch, secondaryTouch);
        markerStopETSPinch.End();
    }

    public void OnHoldStart_EnhancedTouch(Finger finger)
    {
        markerStartETSHold.Begin();
        base.OnHoldStart_EnhancedTouch(finger);
        markerStartETSHold.End();
    }

    public void WhileHolding(Vector2 pointerScreenPosition)
    {
        markerETSHolding.Begin();
        base.WhileHolding(pointerScreenPosition);
        markerStartETSHold.End();
    }

    public void OnHoldEnd(Vector2 pointerScreenPosition)
    {
        markerStopETSHold.Begin();
        base.OnHoldEnd(pointerScreenPosition);
        markerStopETSHold.End();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        markerOnZoom.Begin();
        base.OnZoom(context);
        markerOnZoom.End();
    }

    private void PointerContactAction_EnhancedTouch(Finger finger)
    {
        markerStartETSPointer.Begin();
        base.PointerContactAction_EnhancedTouch(finger);
        markerStartETSPointer.End();
    }
}
