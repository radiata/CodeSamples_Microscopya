using UnityEngine;

public class InputModule_MotorProteinVehicle : I_InputModule
{
    public bool BlockPointer() => true;

    public bool OnClick(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        throw new System.NotImplementedException();
    }

    public bool OnHoldEnd(Vector3 worldPosition, Vector3 cameraForward)
    {
        throw new System.NotImplementedException();
    }

    public bool OnHoldStart(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        throw new System.NotImplementedException();
    }

    public bool OnPinchStart(Vector3 positionTouch1, Vector3 positionTouch2)
    {
        throw new System.NotImplementedException();
    }

    public bool WhilePinching(Vector3 positionTouch1, Vector3 positionTouch2)
    {
        throw new System.NotImplementedException();
    }

    public bool OnPinchEnd(Vector3 positionTouch1, Vector3 positionTouch2)
    {
        throw new System.NotImplementedException();
    }

    public bool OnSwipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        throw new System.NotImplementedException();
    }

    public bool WhileHolding(Vector3 worldPosition, Vector3 cameraForward)
    {
        throw new System.NotImplementedException();
    }

    public bool WhilePinching()
    {
        throw new System.NotImplementedException();
    }

    public bool OnContactStart(Vector3 worldPosition, Vector3 cameraForward, LayerMask targetLayers)
    {
        throw new System.NotImplementedException();
    }

    public bool OnZoom(float zoomDelta)
    {
        CameraZoomController.ChangeZoomBy(zoomDelta * CameraManager.Instance.ZoomStepStrength);

        return true;
    }
}