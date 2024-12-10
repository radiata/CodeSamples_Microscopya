using UnityEngine;

public class InputModule_Menu : I_InputModule
{
    public bool BlockPointer() => false;

    public bool OnClick(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        return false;
    }

    public bool OnHoldEnd(Vector3 worldPosition, Vector3 cameraForward)
    {
        return false;
    }

    public bool OnHoldStart(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        return false;
    }

    public bool OnPinchStart(Vector3 positionTouch1, Vector3 positionTouch2)
    {
        return false;
    }

    public bool WhilePinching(Vector3 positionTouch1, Vector3 positionTouch2)
    {
        return false;
    }

    public bool OnPinchEnd(Vector3 positionTouch1, Vector3 positionTouch2)
    {
        return false;
    }

    public bool OnSwipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        return false;
    }

    public bool WhileHolding(Vector3 worldPosition, Vector3 cameraForward)
    {
        return false;
    }

    public bool OnContactStart(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        return false;
    }

    public bool OnZoom(float zoomDelta)
    {
        return false;
    }
}
