using UnityEngine;

public class CameraManager_StartUp : Base_StartUp
{
    [SerializeField] private Template_SO cameraTemplate_SO;

    [SerializeField] private CameraManager_Settings_SO cameraManager_Settings_SO;

    public static CameraManager activeCameraManager;

    public override void FinalizeProcess()
    {
    }

    protected override void RunProcess()
    {
    }

    protected override bool CheckProcessComplete()
    {
        activeCameraManager = cameraTemplate_SO.InstantiateTemplateObject().GetComponentInChildren<CameraManager>();
        DontDestroyOnLoad(activeCameraManager);

        activeCameraManager.SetCameraManagerSettings(cameraManager_Settings_SO);

        return true;
    }
}
