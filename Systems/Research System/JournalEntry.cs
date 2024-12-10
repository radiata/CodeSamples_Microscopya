using UnityEngine;
using UnityEngine.UI;

public class JournalEntry : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ResearchEntryID researchEntryID;
    [SerializeField] private ResearchJournalSettings_SO researchJournalSettings;
    [SerializeField] private string entryText;

    [Header("Journal Page")]
    [SerializeField] private GameObject journalPagePrefab;
    [SerializeField] private JournalPage journalPage;

    [Header("Linked Components")]
    [SerializeField] private Image buttonBaseImage;
    [SerializeField] private Image buttonSecondaryImage;
    [SerializeField] private Image buttonUnreadImage;
    [SerializeField] private Image buttonHighlightImage;
    [SerializeField] private MultiLanguage_Text_TextMeshProUGUI textTranslationComponent;

    [Header("Label Data")]
    [SerializeField] private TranslatableText_SO lockedText;
    [SerializeField] private TranslatableText_SO labelText;

    public ResearchEntryID ResearchEntryID => researchEntryID;

    internal delegate void OpenEntryEvent(JournalEntry openEntry);
    internal static OpenEntryEvent OnOpenEntry;

    public void OnClick()
    {
        OpenEntry(false);
    }

    public void OpenEntry(bool muteSound)
    {
        bool unlocked = PlayerPrefs_Utilities.GetResearchEntrySaveState(researchEntryID);

        if (muteSound == false)
        {
            AudioController.Instance.PlaySoundEffect(
            researchJournalSettings.OnClickSound(unlocked)
            , false);
        }

        if (unlocked == true)
        {
            journalPage.LoadJournalPage(journalPagePrefab, researchEntryID);
        }
        else
        {
            journalPage.LoadLockedJournalPage();
        }

        ResearchJournal.MarkEntryRead(researchEntryID);
        buttonUnreadImage.enabled = false;

        OnOpenEntry?.Invoke(this);
    }

    private void SetHighlight(JournalEntry entry)
    {
        buttonHighlightImage.enabled = entry == this ? true : false;
    }

    private void OnEnable()
    {
        buttonHighlightImage.enabled = false;
        OnOpenEntry += SetHighlight;

        bool unlocked = PlayerPrefs_Utilities.GetResearchEntrySaveState(researchEntryID);

        if (unlocked == true)
        {
            buttonBaseImage.sprite = researchJournalSettings.EmptyButtonSprite;
            buttonSecondaryImage.enabled = true;

            if(ResearchJournal.IsEntryUnread(researchEntryID) == true)
            {
                buttonUnreadImage.enabled = true;
            }

            textTranslationComponent.SetNewTranslatableText(labelText, true);
        }
        else
        {
            buttonBaseImage.sprite = researchJournalSettings.LockedButtonSprite;
            buttonSecondaryImage.enabled = false;
            buttonUnreadImage.enabled = false;
            textTranslationComponent.SetNewTranslatableText(lockedText, true);
        }
    }

    private void OnDisable()
    {
        OnOpenEntry -= SetHighlight;
    }
}
