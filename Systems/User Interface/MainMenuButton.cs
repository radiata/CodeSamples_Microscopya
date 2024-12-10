using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;
    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);
        UserInterface.Instance.ChangeUserInterfaceLayout(UserInterfaceLayout.None);
        SceneLoader.Instance.LoadMainMenu();
    }
}
