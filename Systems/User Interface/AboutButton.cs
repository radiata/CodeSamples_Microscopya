using UnityEngine;

public class AboutButton : MonoBehaviour
{
    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;
    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);
        SceneLoader.Instance.LoadScene(SceneID.AboutMenu);
    }
}
