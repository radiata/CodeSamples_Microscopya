using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CameraManager_Settings_SO cameraManager_Settings_SO;

    [SerializeField] private Camera activeCamera;
    [SerializeField] private CameraFader cameraFader;

    private CinemachineBrain activeCinemachineBrain;

    public float ZoomStepStrength => cameraManager_Settings_SO.GetCameraZoomStepStrength();

    public delegate void CameraChanged();
    public static event CameraChanged OnCameraChanged;

    public Camera ActiveCamera
    {
        get
        {
            return activeCamera;
        }
        set
        {
            activeCamera = value;
            OnCameraChanged?.Invoke();
        }
    }

    public CinemachineBrain ActiveCinemachineBrain
    {
        get
        {
            return activeCinemachineBrain;
        }
        set
        {
            activeCinemachineBrain = value;
        }
    }

    public void SetCameraManagerSettings(CameraManager_Settings_SO newCameraManager_Settings_SO)
    {
        cameraManager_Settings_SO = newCameraManager_Settings_SO;
    }

    public void FadeOutActiveCamera(float duration = 1f)
    {
        FadeParams fadeParams = new FadeParams() { startAlpha = 0, endAlpha = 1, durationSeconds = duration,  endAsDisabled = false};
        cameraFader.StartFade(fadeParams);
    }

    public void FadeInActiveCamera(float duration = 1f)
    {
        FadeParams fadeParams = new FadeParams() { startAlpha = 1, endAlpha = 0, durationSeconds = duration, endAsDisabled = true };
        cameraFader.StartFade(fadeParams);
    }

    private void UpdateCinemachineBrain()
    {
        ActiveCinemachineBrain = ActiveCamera.GetComponent<CinemachineBrain>();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance.gameObject);
        }
        Instance = this;

        UpdateCinemachineBrain();
        OnCameraChanged?.Invoke();
    }

    private void OnEnable()
    {
        OnCameraChanged += UpdateCinemachineBrain;
    }

    private void OnDisable()
    {
        OnCameraChanged -= UpdateCinemachineBrain;
    }
}
