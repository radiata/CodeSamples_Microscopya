using UnityEngine;
using UnityEngine.UI;

public class HairStyleSelection : MonoBehaviour
{
    [SerializeField] private CharacterHairOptions_SO characterHairOptions_SO;
    [SerializeField] private HairSelection hairStyle;

    [SerializeField] private ExpandedCustomizationField expandedCustomizationField;

    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;

    [SerializeField] private Image iconImage;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);

        characterHairOptions_SO.SetHair(hairStyle);
        expandedCustomizationField.ToggleActiveState(false);
    }

    private void Awake()
    {
        iconImage.sprite = characterHairOptions_SO.GetFirstMatchingHairStyleOrNull(hairStyle).HairIcon;
    }
}
