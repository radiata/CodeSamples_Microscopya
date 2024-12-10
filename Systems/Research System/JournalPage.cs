using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalPage : MonoBehaviour
{
    [SerializeField] private string lockedPageResourcesPath = "lab journal_lock B";
    [SerializeField] private Image lockedPageImageRenderer;

    [SerializeField] private GameObject journalPageParent;

    private GameObject journalPageInstance;
    private ResearchEntryID currentJournalPageID;
    private bool lockedPageLoaded = false;

    [SerializeField] private List<JournalEntry> journalEntries;
    [SerializeField] private JournalIndexScrollToEntry journalIndexScrollToEntry;

    public void LoadJournalPage(GameObject pagePrefab, ResearchEntryID entryID)
    {
        if(currentJournalPageID == entryID)
        {
            return;
        }

        UnloadJournalPages();

        currentJournalPageID = entryID;
        journalPageInstance = Instantiate(pagePrefab, journalPageParent.transform);
    }

    public void LoadLockedJournalPage()
    {
        if (lockedPageLoaded == true)
        {
            return;
        }

        UnloadJournalPages();

        lockedPageLoaded = true;
        lockedPageImageRenderer.sprite = Resources.Load<Sprite>(lockedPageResourcesPath);
        lockedPageImageRenderer.enabled = true;
    }

    public void UnloadJournalPages()
    {
        if(journalPageInstance != null)
        {
            Destroy(journalPageInstance);
        }

        if (lockedPageImageRenderer.sprite != null)
        {
            lockedPageImageRenderer.enabled = false;
            lockedPageImageRenderer.sprite = null;
        }

        lockedPageLoaded = false;
        Resources.UnloadUnusedAssets();
    }

    private void OnEnable()
    {
        if(lockedPageImageRenderer == null)
        {
            lockedPageImageRenderer.enabled = false;
        }
    }

    private IEnumerator Start()
    {
        yield return null;

        if (ResearchJournal.OpenToEntry != ResearchEntryID.None)
        {
            for (int i = 0; i < journalEntries.Count; i++)
            {
                if (journalEntries[i].ResearchEntryID == ResearchJournal.OpenToEntry)
                {
                    journalEntries[i].OpenEntry(true);
                    journalIndexScrollToEntry.ScrollToEntry(journalEntries[i]);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < journalEntries.Count; i++)
            {
                if (journalEntries[i].ResearchEntryID == ResearchJournal.LatestUnlock)
                {
                    journalEntries[i].OpenEntry(true);
                    journalIndexScrollToEntry.ScrollToEntry(journalEntries[i]);
                    break;
                }
            }
        }

        ResearchJournal.SetOpenToEntry(ResearchEntryID.None);
    }
}
