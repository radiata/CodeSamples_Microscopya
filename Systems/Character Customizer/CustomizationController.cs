using UnityEngine;

public class CustomizationController : MonoBehaviour
{
    [SerializeField] private CharacterColorOptions_SO characterColorOptions_SO;
    [SerializeField] private CharacterHairOptions_SO characterHairOptions_SO;

    [SerializeField] private CustomizationField hairCustomizationField;
    [SerializeField] private CustomizationField skinCustomizationField;
    [SerializeField] private CustomizationField coatCustomizationField;
    [SerializeField] private CustomizationField dressCustomizationField;
    [SerializeField] private CustomizationField shoesCustomizationField;

    [SerializeField] private Color activeButtonColor;

    [SerializeField] private FlexibleColorPicker flexibleColorPicker;

    private CustomizationField activeCustomizationField;
    private CustomizationFields selectedCustomizationField;

    public bool UnsavedChanges()
    {
        return characterColorOptions_SO.UnsavedChanges() || characterHairOptions_SO.UnsavedChanges();
    }

    public void ChangeSelectedCustomizationField(CustomizationFields customizationField)
    {
        selectedCustomizationField = customizationField;
        Color currentColor = Color.white;

        hairCustomizationField.ChangeBackgroundColor(Color.white);
        skinCustomizationField.ChangeBackgroundColor(Color.white);
        coatCustomizationField.ChangeBackgroundColor(Color.white);
        dressCustomizationField.ChangeBackgroundColor(Color.white);
        shoesCustomizationField.ChangeBackgroundColor(Color.white);

        switch (customizationField)
        {
            case CustomizationFields.None:
                activeCustomizationField = null;
                break;
            case CustomizationFields.Hair:
                activeCustomizationField = hairCustomizationField;
                currentColor = characterColorOptions_SO.HairColor;
                break;
            case CustomizationFields.Skin:
                activeCustomizationField = skinCustomizationField;
                currentColor = characterColorOptions_SO.SkinColor;
                break;
            case CustomizationFields.Coat:
                activeCustomizationField = coatCustomizationField;
                currentColor = characterColorOptions_SO.CoatColor;
                break;
            case CustomizationFields.Dress:
                activeCustomizationField = dressCustomizationField;
                currentColor = characterColorOptions_SO.DressColor;
                break;
            case CustomizationFields.Shoes:
                activeCustomizationField = shoesCustomizationField;
                currentColor = characterColorOptions_SO.ShoeColor;
                break;
        }

        if (activeCustomizationField != null)
        {
            activeCustomizationField.ChangeBackgroundColor(activeButtonColor);
        }

        flexibleColorPicker.color = currentColor;
    }

    public void SetCustomizationFieldColor(Color color)
    {
        switch (selectedCustomizationField)
        {
            case CustomizationFields.None:
                DebugWrapper.LogWarning("No Customization Field Selected!", gameObject);
                return;
            case CustomizationFields.Hair:
                characterColorOptions_SO.SetHairColor(color);
                break;
            case CustomizationFields.Skin:
                characterColorOptions_SO.SetSkinColor(color);
                break;
            case CustomizationFields.Coat:
                characterColorOptions_SO.SetCoatColor(color);
                break;
            case CustomizationFields.Dress:
                characterColorOptions_SO.SetDressColor(color);
                break;
            case CustomizationFields.Shoes:
                characterColorOptions_SO.SetShoeColor(color);
                break;
        }

        activeCustomizationField.ChangeIconColor(color);
    }

    public void SaveCustomizationOptions()
    {
        characterColorOptions_SO.SaveColors();
        characterHairOptions_SO.SaveHairOptions();
    }

    public void SetToDefaultCustomizationOptions()
    {
        characterColorOptions_SO.SetToDefaultColors();
        characterHairOptions_SO.SetToDefaultHair();
        InitializeIconColors();
        flexibleColorPicker.color = activeCustomizationField.IconColor;
    }

    public void RevertChanges()
    {
        characterColorOptions_SO.LoadColors(false);
        characterHairOptions_SO.LoadHairOptions(false);
    }

    private void Awake()
    {
        InitializeIconColors();
    }

    private void Start()
    {
        SelectStartingButton();
    }

    private void InitializeIconColors()
    {
        hairCustomizationField.ChangeIconColor(characterColorOptions_SO.HairColor);
        skinCustomizationField.ChangeIconColor(characterColorOptions_SO.SkinColor);
        coatCustomizationField.ChangeIconColor(characterColorOptions_SO.CoatColor);
        dressCustomizationField.ChangeIconColor(characterColorOptions_SO.DressColor);
        shoesCustomizationField.ChangeIconColor(characterColorOptions_SO.ShoeColor);
    }

    private void SelectStartingButton()
    {
        flexibleColorPicker.startingColor = characterColorOptions_SO.HairColor;
        hairCustomizationField.SetSelected();
    }
}
