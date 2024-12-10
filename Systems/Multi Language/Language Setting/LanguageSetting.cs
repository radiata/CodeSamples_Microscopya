using UnityEngine;

public static class LanguageSetting
{
    private static Languages currentLanguage = Languages.Uninitialized;
    public static Languages CurrentLanguage => currentLanguage;

    private static Languages defaultLanguage = Languages.English;

    public delegate void LanguageSettingEvent(Languages language);
    public static LanguageSettingEvent OnLanguageChanged;

    public static void ChangeLanguageSetting(Languages language)
    {
        if(currentLanguage == language)
        {
            return;
        }

        currentLanguage = language;

        SaveLanguageSetting();

        OnLanguageChanged?.Invoke(currentLanguage);
    }

    public static void SaveLanguageSetting()
    {
        PlayerPrefs_Utilities.SetLanguageSaveState((int)CurrentLanguage);
    }

    public static void LoadLanguageSetting()
    {
        Languages loadedLanguage = (Languages) PlayerPrefs_Utilities.GetLanguageSaveState();

        if (loadedLanguage == Languages.Uninitialized)
        {
            loadedLanguage = defaultLanguage;
        }

        ChangeLanguageSetting(loadedLanguage);
    }
}
