using Cinemachine;
using UnityEngine;

public class CharacterCameraManager : MonoBehaviour
{
    [SerializeField] private CameraZoomController cameraZoomController;
    [SerializeField] private CameraTiltController cameraTiltController;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineConfiner cinemachineConfiner;
    
    public void SetCameraZoomControllerValues(float minimumZoomValue, float maximumZoomValue, float zoomSmoothingSpeed, float defaulZoomValue)
    {
        cameraZoomController.SetLocalMinimumZoomValue(minimumZoomValue);
        cameraZoomController.SetLocalMaximumZoomValue(maximumZoomValue);
        cameraZoomController.SetLocalZoomSmoothingSpeed(zoomSmoothingSpeed);
        cameraZoomController.SetDefaultZoomValue(defaulZoomValue);
        cameraZoomController.ResetZoom();
    }

    public void SetVirtualCameraFramingTransposerValues()
    {
        return;
        CinemachineFramingTransposer framingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        if(framingTransposer == null)
        {
            framingTransposer = cinemachineVirtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    public void SetCameraTiltControllerValues(float tiltSmoothingSpeed, float tiltWeight)
    {
        cameraTiltController.SetControllerValues(tiltSmoothingSpeed, tiltWeight);
    }

    private void Awake()
    {
        cinemachineConfiner.m_BoundingShape2D = MapConfinesChecker.FindMapConfines();
    }
}
