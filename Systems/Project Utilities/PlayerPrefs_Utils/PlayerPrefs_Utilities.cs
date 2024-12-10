using UnityEngine;

public static class PlayerPrefs_Utilities
{
    private const string ColorR = ".R";
    private const string ColorG = ".G";
    private const string ColorB = ".B";
    private const string ColorA = ".A";

    private const string HairSelectionKey = "HairSelection";
    private const string CameraTiltAccessibilityKey = "CameraTiltDisabled";
    private const string LanguageKey = "Language";
    private const string MainVolumeKey = "MainVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundEffectsVolumeKey = "SoundEffectsVolume";
    private const string MutedVolumeKey = "MutedVolume";

    public static void SetColorToPreferencesKey(string preferencesKey, Color newColor)
    {
        PlayerPrefs.SetFloat(preferencesKey + ColorR, newColor.r);
        PlayerPrefs.SetFloat(preferencesKey + ColorG, newColor.g);
        PlayerPrefs.SetFloat(preferencesKey + ColorB, newColor.b);
        PlayerPrefs.SetFloat(preferencesKey + ColorA, newColor.a);
    }

    public static Color GetColorFromPreferencesKey(string preferencesKey, Color defaultColor)
    {
        var color = new Color(
                PlayerPrefs.GetFloat(preferencesKey + ColorR, defaultColor.r),
                PlayerPrefs.GetFloat(preferencesKey + ColorG, defaultColor.g),
                PlayerPrefs.GetFloat(preferencesKey + ColorB, defaultColor.b),
                PlayerPrefs.GetFloat(preferencesKey + ColorA, defaultColor.a));
        return color;
    }

    public static void SetHairCustomization(HairSelection hairSelection)
    {
        PlayerPrefs.SetInt(HairSelectionKey, (int)hairSelection);
    }

    public static HairSelection GetHairCustomization()
    {
        int selection = PlayerPrefs.GetInt(HairSelectionKey, 0);
        //validate selection or return straight
        if (selection < (int)HairSelection.Straight || selection > (int)HairSelection.None)
        {
            return HairSelection.Straight;
        }
        return (HairSelection)selection;
    }

    public static void SetCameraTiltAccessibilitySetting(bool isDisabled)
    {
        PlayerPrefs.SetInt(CameraTiltAccessibilityKey, isDisabled ? 1 : 0);
    }

    public static bool GetCameraTiltAccessibilitySetting()
    {
        return PlayerPrefs.GetInt(CameraTiltAccessibilityKey, 0) == 1 ? true : false;
    }

    public static void SetPuzzleSaveState(PuzzleKey puzzleKey, bool solvedState)
    {
        string keyString = StringMap_PuzzleKey.ConvertToString(puzzleKey);
        PlayerPrefs.SetInt(keyString, solvedState ? 1 : 0);
    }

    public static bool GetPuzzleSaveState(PuzzleKey puzzleKey)
    {
        string keyString = StringMap_PuzzleKey.ConvertToString(puzzleKey);
        return PlayerPrefs.GetInt(keyString, 0) == 1 ? true : false;
    }

    public static void SetResearchEntrySaveState(ResearchEntryID researchEntryID, bool unlockedState)
    {
        string keyString = StringMap_ResearchEntryID.ConvertToString(researchEntryID);
        PlayerPrefs.SetInt(keyString, unlockedState ? 1 : 0);
    }

    public static bool GetResearchEntrySaveState(ResearchEntryID researchEntryID)
    {
        string keyString = StringMap_ResearchEntryID.ConvertToString(researchEntryID);
        return PlayerPrefs.GetInt(keyString, 0) == 1 ? true : false;
    }

    public static void SetLanguageSaveState(int currentLanguage)
    {
        PlayerPrefs.SetInt(LanguageKey, currentLanguage);
    }

    public static int GetLanguageSaveState()
    {
        return PlayerPrefs.GetInt(LanguageKey, -1);
    }

    public static void SetVolumeSettings(VolumeSettings volumeSettings)
    {
        PlayerPrefs.SetFloat(MainVolumeKey, volumeSettings.MainVolume);
        PlayerPrefs.SetFloat(MusicVolumeKey, volumeSettings.MusicVolume);
        PlayerPrefs.SetFloat(SoundEffectsVolumeKey, volumeSettings.SoundEffectsVolume);
        PlayerPrefs.SetInt(MutedVolumeKey, volumeSettings.Muted ? 1 : 0);
    }

    public static VolumeSettings GetVolumeSettings()
    {
        return new VolumeSettings(
            PlayerPrefs.GetFloat(MainVolumeKey, -1),
            PlayerPrefs.GetFloat(MusicVolumeKey, -1),
            PlayerPrefs.GetFloat(SoundEffectsVolumeKey, -1),
            PlayerPrefs.GetInt(MutedVolumeKey, -1) <= 0 ? false : true);
    }
}
