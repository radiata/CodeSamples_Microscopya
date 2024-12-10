using UnityEngine;
using TMPro;

public class MultiLanguage_Text_TextMeshProUGUI : Base_MultiLanguage
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TranslatableText_SO translatableText;

    public TranslatableText_SO CurrentTranslatableText => translatableText;

    protected override void UpdateTargetObject(Languages language)
    {
        text.text = translatableText.GetTranslation(language);
    }

    public void SetNewTranslatableText(TranslatableText_SO newTranslatableText, bool updateTargetObject = true)
    {
        translatableText = newTranslatableText;
        if (updateTargetObject == true)
        {
            UpdateTargetObject(LanguageSetting.CurrentLanguage);
        }
    }
}
