using UnityEngine;
using UnityEngine.UI;

public class ActiveHairStyleIcon : MonoBehaviour
{
    [SerializeField] private CharacterHairOptions_SO characterHairOptions_SO;
    [SerializeField] private Image iconImage;

    private void OnEnable()
    {
        UpdateHairStyleIcon();

        CharacterHairOptions_SO.OnCharacterHairChanged -= UpdateHairStyleIcon;
        CharacterHairOptions_SO.OnCharacterHairChanged += UpdateHairStyleIcon;
    }

    private void OnDisable()
    {
        CharacterHairOptions_SO.OnCharacterHairChanged -= UpdateHairStyleIcon;
    }

    private void UpdateHairStyleIcon()
    {
        iconImage.sprite = characterHairOptions_SO.CurrentHairStyle.HairIcon;
    }
}
