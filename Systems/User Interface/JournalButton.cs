using UnityEngine;
using UnityEngine.UI;

public class JournalButton : MonoBehaviour
{
    public static JournalButton Instance;

    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;

    [SerializeField] private bool updateUserInterface = true;
    [SerializeField] private Image buttonImageRenderer;

    [SerializeField] private Sprite journalButton_Default;
    [SerializeField] private Sprite journalButton_Unread;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);
        SceneLoader.Instance.LoadScene(SceneID.Journal);
    }

    private void OnEnable()
    {
        UpdateUserInterfaceElements(ResearchEntryID.None);
        ResearchUnlocks.OnResearchEntryUnlocked += UpdateUserInterfaceElements;
        ResearchJournal.OnResearchJournalOpened += UpdateUserInterfaceElements;
    }

    private void OnDisable()
    {
        ResearchUnlocks.OnResearchEntryUnlocked -= UpdateUserInterfaceElements;
        ResearchJournal.OnResearchJournalOpened -= UpdateUserInterfaceElements;
    }

    private void UpdateUserInterfaceElements(ResearchEntryID _)
    {
        UpdateUserInterfaceElements();
    }
    private void UpdateUserInterfaceElements()
    {
        if(updateUserInterface == false)
        {
            return;
        }

        buttonImageRenderer.sprite =
            ResearchJournal.IsAnyEntryUnread == true ? journalButton_Unread : journalButton_Default;
    }

    private void Awake()
    {
        if (Instance == null
            && SceneLoader.Instance.SceneLibrary.GetSceneID(gameObject.scene.name) == SceneID.UserInterface)
        {
            Instance = this;
        }

        DebugWrapper.Log("Transfer the instance references to UserInterface?", gameObject);
    }
}
