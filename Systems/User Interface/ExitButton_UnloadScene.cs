using UnityEngine;

public class ExitButton_UnloadScene : MonoBehaviour
{
    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericCancel;
    [SerializeField] private SceneID sceneToUnload;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);
        SceneLoader.Instance.UnloadScene(sceneToUnload);
    }
}
