using UnityEngine;

public interface I_InputModule
{
    public bool BlockPointer();

    public bool OnPinchStart(Vector3 positionTouch1, Vector3 positionTouch2);
    public bool WhilePinching(Vector3 positionTouch1, Vector3 positionTouch2);
    public bool OnPinchEnd(Vector3 positionTouch1, Vector3 positionTouch2);
    public bool OnSwipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward, LayerMask targetLayers);
    public bool OnClick(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers);
    public bool OnHoldStart(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers);
    public bool WhileHolding(Vector3 worldPosition, Vector3 cameraForward);
    public bool OnHoldEnd(Vector3 worldPosition, Vector3 cameraForward);
    public bool OnContactStart(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers);
    public bool OnZoom(float zoomDelta);
}
