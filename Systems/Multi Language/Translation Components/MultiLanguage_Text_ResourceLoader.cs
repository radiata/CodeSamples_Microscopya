using UnityEngine;

public class MultiLanguage_Text_ResourceLoader : Base_MultiLanguage
{
    [SerializeField] private PanelResourceLoading resourceLoader;
    [SerializeField] private TranslatableText_SO translatablePath;

    protected override void UpdateTargetObject(Languages language)
    {
        resourceLoader.SetPanelPath(translatablePath.GetTranslation(language), false);
    }

    public void SetNewTranslatableText(TranslatableText_SO newTranslatableText, bool updateTargetObject = true)
    {
        translatablePath = newTranslatableText;
        if (updateTargetObject == true)
        {
            UpdateTargetObject(LanguageSetting.CurrentLanguage);
        }
    }

}
