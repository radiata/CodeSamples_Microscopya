using UnityEngine;

public class LanguageButton : MonoBehaviour
{
    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;

    [SerializeField] private LanguageToggle languageToggle;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);
        languageToggle.NextLanguage();
    }
}
