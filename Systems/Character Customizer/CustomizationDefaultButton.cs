using UnityEngine;

public class CustomizationDefaultButton : MonoBehaviour
{
    [SerializeField] CustomizationController customizationController;
    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);

        customizationController.SetToDefaultCustomizationOptions();
    }
}
