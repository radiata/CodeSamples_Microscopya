using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ResearchModeCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Volume researchVolume;

    [SerializeField] private GameObject researchModeStackCamera;

    private UniversalAdditionalCameraData cameraData;

    private void EnableResearchModeCamera()
    {
        researchVolume.enabled = true;
        cameraData.renderPostProcessing = true;
        researchModeStackCamera.SetActive(true);
    }

    private void DisableResearchModeCamera()
    {
        researchVolume.enabled = false;
        cameraData.renderPostProcessing = false;
        researchModeStackCamera.SetActive(false);
    }

    private void OnResearchModeStateChanged(bool isEnabled)
    {
        if (isEnabled == true)
        {
            EnableResearchModeCamera();
        }
        else
        {
            DisableResearchModeCamera();
        }
    }


    private void Awake()
    {
        cameraData = mainCamera.GetUniversalAdditionalCameraData();
        DisableResearchModeCamera();
    }

    private void OnEnable()
    {
        ResearchModeState.OnResearchModeStateChanged += OnResearchModeStateChanged;
    }

    private void OnDisable()
    {
        ResearchModeState.OnResearchModeStateChanged -= OnResearchModeStateChanged;
    }
}
