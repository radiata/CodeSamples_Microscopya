using UnityEngine;

public class CustomizeButton : MonoBehaviour
{
    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);
        SceneLoader.Instance.LoadScene(SceneID.CustomizationMenu);
    }
}
