using UnityEngine;

public class ApplicationSettings_StartUp : Base_StartUp
{
    [SerializeField] private ApplicationSettings_SO applicationSettings;

    protected override void RunProcess()
    {
        ApplicationSettings.Initialize(applicationSettings.TargetFrameRate, applicationSettings.EnableTargetFrameRate, applicationSettings.SleepTimeout);
    }

    protected override bool CheckProcessComplete()
    {
        return true;
    }

   
    public override void FinalizeProcess()
    {
    }
}
