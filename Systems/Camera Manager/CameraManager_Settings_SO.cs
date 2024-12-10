using UnityEngine;

[CreateAssetMenu(fileName = "Camera Manager Settings", menuName = "Camera Manager/Camera Manager Scriptable Objects/Camera Manager Settings")]
public class CameraManager_Settings_SO : ScriptableObject
{
    private float? cameraZoomStepStrength = null;
    [SerializeField] private float cameraZoomStepStrength_Default = .175f;
    [SerializeField] private float cameraZoomStepStrength_Mobile = .175f;
    [SerializeField] private float cameraZoomStepStrength_WebGL = .175f;

    public float GetCameraZoomStepStrength()
    {
        if(cameraZoomStepStrength == null)
        {
            #if UNITY_WEBGL
                cameraZoomStepStrength = cameraZoomStepStrength_WebGL;
            #elif UNITY_IOS
                cameraZoomStepStrength = cameraZoomStepStrength_Mobile;
            #elif UNITY_ANDROID
                cameraZoomStepStrength = cameraZoomStepStrength_Mobile;
            #else
                cameraZoomStepStrength = cameraZoomStepStrength_Default;
            #endif
        }

        return cameraZoomStepStrength.Value;
    }
}
