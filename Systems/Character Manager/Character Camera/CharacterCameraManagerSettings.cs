using UnityEngine;

[System.Serializable]
public class CharacterCameraManagerSettings
{
    [SerializeField] private float zoomSmoothingSpeed;
    [SerializeField] private float defaultZoomValue;
    [SerializeField] private float minimumZoomValue;
    [SerializeField] private float maximumZoomValue;

    [SerializeField] private float tiltSmoothingSpeed;
    [SerializeField] private float tiltWeight;

    public static string ZoomSmoothingSpeedVariableName => nameof(zoomSmoothingSpeed);
    public static string DefaultZoomValueVariableName => nameof(defaultZoomValue);
    public static string MinimumZoomValueVariableName => nameof(minimumZoomValue);
    public static string MaximumZoomValueVariableName => nameof(maximumZoomValue);

    public static string TiltSmoothingSpeedVariableName => nameof(tiltSmoothingSpeed);
    public static string TiltWeightVariableName => nameof(tiltWeight);

    public void ApplyCharacterCameraManagerSettings(CharacterCameraManager characterCameraManager)
    {
        characterCameraManager.SetCameraZoomControllerValues(minimumZoomValue, maximumZoomValue, zoomSmoothingSpeed, defaultZoomValue);
        characterCameraManager.SetVirtualCameraFramingTransposerValues();
        characterCameraManager.SetCameraTiltControllerValues(tiltSmoothingSpeed, tiltWeight);
    }
}
