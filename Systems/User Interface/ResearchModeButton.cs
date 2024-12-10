using UnityEngine;
using UnityEngine.UI;

public class ResearchModeButton : MonoBehaviour
{
    public static ResearchModeButton Instance; 

    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;

    [SerializeField] private bool updateUserInterface = true;
    [SerializeField] private Image imageRenderer;

    [SerializeField] private Sprite researchModeOnSprite;
    [SerializeField] private Sprite researchModeOffSprite;

    [SerializeField] private GameObject magnifyingGlass;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);

        ResearchModeState.ChangeState(!ResearchModeState.ResearchModeEnabledState);
    }

    private void OnEnable()
    {
        UpdateUserInterfaceElements(ResearchModeState.ResearchModeEnabledState);

        ResearchModeState.OnResearchModeStateChanged += UpdateUserInterfaceElements;
    }

    private void OnDisable()
    {
        ResearchModeState.OnResearchModeStateChanged -= UpdateUserInterfaceElements;
    }

    private void UpdateUserInterfaceElements(bool researchModeEnabled)
    {
        if(updateUserInterface == false)
        {
            return;
        }

        imageRenderer.sprite = researchModeEnabled == true ? researchModeOnSprite : researchModeOffSprite;
        magnifyingGlass.SetActive(researchModeEnabled);
    }

    private void Awake()
    {
        if(Instance == null
            && SceneLoader.Instance.SceneLibrary.GetSceneID(gameObject.scene.name) == SceneID.UserInterface)
        {
            Instance = this;
        }

        DebugWrapper.Log("Transfer the instance references to UserInterface?", gameObject);
    }
}
