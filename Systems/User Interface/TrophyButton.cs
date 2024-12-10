using UnityEngine;
using UnityEngine.UI;

public class TrophyButton : MonoBehaviour
{
    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;

    [SerializeField] private bool updateUserInterface = true;
    [SerializeField] private Image buttonImageRenderer;

    [SerializeField] private Sprite trophyButton_Default;
    [SerializeField] private Sprite trophyButton_Unread;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);
        SceneLoader.Instance.LoadScene(SceneID.TrophyMenu);
    }

    private void OnEnable()
    {
        UpdateUserInterfaceElements();
        TrophyUnlocked.OnTrophyUnlocked += UpdateUserInterfaceElements;
        TrophyUnlocked.OnTrophyRead += UpdateUserInterfaceElements;
    }

    private void OnDisable()
    {
        TrophyUnlocked.OnTrophyUnlocked -= UpdateUserInterfaceElements;
        TrophyUnlocked.OnTrophyRead -= UpdateUserInterfaceElements;
    }

    private void UpdateUserInterfaceElements()
    {
        if (updateUserInterface == false)
        {
            return;
        }

        buttonImageRenderer.sprite =
            TrophyUnlocked.IsTrophyUnread == true ? trophyButton_Unread : trophyButton_Default;
    }
}
