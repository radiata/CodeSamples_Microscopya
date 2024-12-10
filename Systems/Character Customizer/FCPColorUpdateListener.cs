using UnityEngine;

public class FCPColorUpdateListener : MonoBehaviour
{
    [SerializeField] private FlexibleColorPicker flexibleColorPicker;
    [SerializeField] private CustomizationController customizationController;
    [SerializeField] private CustomizationExitButton exitButton;

    private Color lastColor;

    private void Update()
    {
        if(flexibleColorPicker.color != lastColor)
        {
            customizationController.SetCustomizationFieldColor(flexibleColorPicker.color);
            lastColor = flexibleColorPicker.color;
            exitButton.ResetWarning();
        }
    }
}
