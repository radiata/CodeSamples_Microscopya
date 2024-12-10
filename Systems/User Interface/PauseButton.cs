using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Image imageRenderer;
    [SerializeField] private Sprite pauseImage;
    [SerializeField] private Sprite resumeImage;

    [SerializeField] private SoundEffect onClickSound;

    private UserInterfaceLayout previousLayout = UserInterfaceLayout.Uninitialized;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);

        if (UserInterface.Instance.ActiveUserInterfaceLayoutType == UserInterfaceLayout.PauseMenu)
        {
            UserInterface.Instance.ChangeUserInterfaceLayout(previousLayout);
            previousLayout = UserInterfaceLayout.Uninitialized;
        }
        else
        {
            previousLayout = UserInterface.Instance.ActiveUserInterfaceLayoutType;
            UserInterface.Instance.ChangeUserInterfaceLayout(UserInterfaceLayout.PauseMenu);
        }
    }

    public void UpdateImage()
    {
        if (UserInterface.Instance.ActiveUserInterfaceLayoutType == UserInterfaceLayout.PauseMenu)
        {
            imageRenderer.sprite = resumeImage;
        }
        else
        {
            imageRenderer.sprite = pauseImage;
        }
    }
}
