using UnityEngine;

public abstract class Base_MultiLanguage : MonoBehaviour
{
    private Languages currentLanguage = Languages.Uninitialized;

    private void UpdateLanguage(Languages language)
    {
        if(language != currentLanguage)
        {
            UpdateTargetObject(language);
            currentLanguage = language;
        }
    }

    /// <summary>
    /// This is called in the base class when language is updated.
    /// Updates the target object to the specified language.
    /// </summary>
    /// <param name="language"></param>
    protected abstract void UpdateTargetObject(Languages language);

    private void OnEnable()
    {
        UpdateLanguage(LanguageSetting.CurrentLanguage);
        LanguageSetting.OnLanguageChanged += UpdateLanguage;
    }

    private void OnDisable()
    {
        LanguageSetting.OnLanguageChanged -= UpdateLanguage;
    }
}
