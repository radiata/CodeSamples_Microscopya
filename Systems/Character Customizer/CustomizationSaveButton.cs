using UnityEngine;

public class CustomizationSaveButton : MonoBehaviour
{
    [SerializeField] CustomizationController customizationController;
    [SerializeField] private CustomizationMenu_SaveButton_TextSelection saveButtonTextSelector;

    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;

    [SerializeField] private CustomizationExitButton exitButton;

    public delegate void CustomizationSavedEvent(bool playGame);
    public static event CustomizationSavedEvent OnCustomizationSaved;

    private static bool loadedFromStartNewGame = false;
    private bool alternateSaveText = false;
    public bool AlternateSaveText => alternateSaveText;

    public static void SetFlagLoadingFromStartNewGame()
    {
        loadedFromStartNewGame = true;
    }

    private void Awake()
    {
        alternateSaveText = loadedFromStartNewGame;
        loadedFromStartNewGame = false;
        saveButtonTextSelector.Initialize(alternateSaveText);
    }

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);

        customizationController.SaveCustomizationOptions();
        exitButton.ResetWarning();

        OnCustomizationSaved?.Invoke(alternateSaveText);
    }
}
