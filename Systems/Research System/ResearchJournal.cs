using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResearchJournal : MonoBehaviour
{
    private static ResearchEntryID openToEntry = ResearchEntryID.None;
    public static ResearchEntryID OpenToEntry => openToEntry;

    private static ResearchEntryID latestUnlock = ResearchEntryID.None;
    public static ResearchEntryID LatestUnlock => latestUnlock;

    public delegate void ResearchJournalOpenedEvent();
    public static ResearchJournalOpenedEvent OnResearchJournalOpened;

    public static bool IsAnyEntryUnread => unreadEntries.Count > 0;

    private static List<ResearchEntryID> unreadEntries = new List<ResearchEntryID>();

    public static bool IsEntryUnread(ResearchEntryID researchEntryID)
    {
        if (unreadEntries.Contains(researchEntryID) == true)
        {
            return true;
        }

        return false;
    }

    public static void AddUnreadEntry(ResearchEntryID researchEntryID)
    {
        unreadEntries.Add(researchEntryID);
        SetLatestUnlock(researchEntryID);
    }

    public static void MarkEntryRead(ResearchEntryID researchEntryID)
    {
        unreadEntries.Remove(researchEntryID);
    }

    public static void SetOpenToEntry(ResearchEntryID researchEntryID)
    {
        openToEntry = researchEntryID;
    }

    private static void SetLatestUnlock(ResearchEntryID researchEntryID)
    {
        latestUnlock = researchEntryID;
    }

    private void Awake()
    {
        if (latestUnlock != ResearchEntryID.None)
        {
            return;
        }

        IEnumerable<ResearchEntryID> values =
            Enum.GetValues(typeof(ResearchEntryID)).Cast<ResearchEntryID>();

        foreach (ResearchEntryID value in values)
        {
            if (ResearchUnlocks.IsResearchEntryUnlocked(value) == true)
            {
                SetLatestUnlock(value);
                return;
            }
        }
    }

    private void OnEnable()
    {
        OnResearchJournalOpened?.Invoke();
    }
}
