using UnityEngine;

public class PointerInteractable_References_StartUp : Base_StartUp
{

    [SerializeField] private PointerInteractable_References_SO loadedSettings;

    public override void FinalizeProcess()
    {

    }

    protected override void RunProcess()
    {
        
    }

    protected override bool CheckProcessComplete()
    {
        PointerInteractable_References.CharacterNavigationLayers = loadedSettings.GetCharacterNavigationLayers();
        PointerInteractable_References.PointerDetectionLayers = loadedSettings.GetPointerDetectionLayers();
        PointerInteractable_References.ResearchMode_PointerDetectionLayers = loadedSettings.GetResearchMode_PointerDetectionLayers();

        return true;
    }
}
