using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour
{
    [SerializeField] private Image imageRenderer;
    [SerializeField] private Sprite hintOffSprite;
    [SerializeField] private Sprite hintOnSprite;

    [SerializeField] private SoundEffect onClickSound;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);

        HintState.ChangeState(!HintState.HintsEnabledState);
    }

    private void OnEnable()
    {
        UpdateImage(HintState.HintsEnabledState);

        HintState.OnHintStateChanged += UpdateImage;
    }

    private void OnDisable()
    {
        HintState.OnHintStateChanged -= UpdateImage;
    }

    private void UpdateImage(bool hintsEnabled)
    {
        imageRenderer.sprite = hintsEnabled == true ? hintOnSprite : hintOffSprite;
    }
}
