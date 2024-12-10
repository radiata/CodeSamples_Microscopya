using UnityEngine;

public class AudioController_StartUp : Base_StartUp
{
    [SerializeField] private Template_SO audioControllerTemplate_SO;

    [SerializeField] private AudioController_Settings_SO audioController_Settings_SO;

    public static AudioController activeAudioController;
    public override void FinalizeProcess()
    {
    }

    protected override void RunProcess()
    {
    }

    protected override bool CheckProcessComplete()
    {
        activeAudioController = audioControllerTemplate_SO.InstantiateTemplateObject().GetComponentInChildren<AudioController>();
        DontDestroyOnLoad(activeAudioController);

        activeAudioController.InitializeAudioControllerSettings(audioController_Settings_SO);

        return true;
    }
}
