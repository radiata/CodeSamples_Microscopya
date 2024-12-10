using UnityEngine;

public class CustomizationMenu_SaveButton_TextSelection : MonoBehaviour
{
    [SerializeField] private MultiLanguage_Text_TextMeshProUGUI multiLanguageText;

    [SerializeField] private TranslatableText_SO primaryText;
    [SerializeField] private TranslatableText_SO alternateText;

    public void Initialize(bool alternateSaveText)
    {
        if (alternateSaveText == true)
        {
            SetAlternateText();
        }
        else
        {
            SetPrimaryText();
        }
    }

    public void SetPrimaryText()
    {
        multiLanguageText.SetNewTranslatableText(primaryText, true);
    }

    public void SetAlternateText()
    {
        multiLanguageText.SetNewTranslatableText(alternateText, true);
    }
}
