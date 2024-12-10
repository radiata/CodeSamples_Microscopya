using System;

public static class ResearchUnlocks
{
    //convert these options to SO for easy modification
    private static SoundEffect unlockSound = SoundEffect.TrophyNotification;
    private static SoundEffect openJournalSound = SoundEffect.GenericConfirm;

    public delegate void ResearchEntryUnlockedEvent(ResearchEntryID researchEntryID);
    public static event ResearchEntryUnlockedEvent OnResearchEntryUnlocked;

    public static void UnlockResearchEntry(ResearchEntryID researchEntryID, ResearchObject researchObject)
    {

        if (IsResearchEntryUnlocked(researchEntryID) == true)
        {
            ResearchJournal.SetOpenToEntry(researchEntryID);
            AudioController.Instance.PlaySoundEffect(openJournalSound, false);
            SceneLoader.Instance.LoadScene(SceneID.Journal);
            return;
        }

        PlayerPrefs_Utilities.SetResearchEntrySaveState(researchEntryID, true);
        ResearchJournal.AddUnreadEntry(researchEntryID);
        AudioController.Instance.PlaySoundEffect(unlockSound, false);

        if (researchObject != null)
        {
            researchObject.PlayUnlockAnimation();
        }

        OnResearchEntryUnlocked?.Invoke(researchEntryID);
    }

    public static bool IsResearchEntryUnlocked(ResearchEntryID researchEntryID)
    {
        return PlayerPrefs_Utilities.GetResearchEntrySaveState(researchEntryID);
    }

    public static void UnlockAllEntries()
    {
        ResearchEntryID[] researchEntries = (ResearchEntryID[])Enum.GetValues(typeof(ResearchEntryID));

        foreach (ResearchEntryID researchEntryID in researchEntries)
        {
            PlayerPrefs_Utilities.SetResearchEntrySaveState(researchEntryID, true);
        }
    }

    public static void LockAllEntries()
    {
        ResearchEntryID[] researchEntries = (ResearchEntryID[])Enum.GetValues(typeof(ResearchEntryID));

        foreach (ResearchEntryID researchEntryID in researchEntries)
        {
            PlayerPrefs_Utilities.SetResearchEntrySaveState(researchEntryID, false);
        }
    }
}
