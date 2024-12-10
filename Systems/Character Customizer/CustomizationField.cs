using UnityEngine;
using UnityEngine.UI;

public class CustomizationField : MonoBehaviour
{
    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;
    [SerializeField] private CustomizationFields customizationField;
    [SerializeField] private ExpandedCustomizationField expandedCustomizationField;
    [SerializeField] private CustomizationController customizationController;

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;

    public Color IconColor => iconImage.color;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);
        customizationController.ChangeSelectedCustomizationField(customizationField);

        if(expandedCustomizationField != null)
        {
            expandedCustomizationField.ToggleActiveState();
        }
    }

    public void SetSelected()
    {
        customizationController.ChangeSelectedCustomizationField(customizationField);
    }

    public void ChangeBackgroundColor(Color color)
    {
        backgroundImage.color = color;
    }

    public void ChangeIconColor(Color color)
    {
        iconImage.color = color;
    }
}
