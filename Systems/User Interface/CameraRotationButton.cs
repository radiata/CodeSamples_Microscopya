using UnityEngine;
using UnityEngine.UI;

public class CameraRotationButton : MonoBehaviour
{
    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;

    [SerializeField] private bool updateUserInterface = true;
    [SerializeField] private Image imageRenderer;

    [SerializeField] private Sprite tiltOffSprite;
    [SerializeField] private Sprite tiltOnSprite;


    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);
        CameraTiltControl_AccessibilitySetting.ChangeTiltState(!CameraTiltControl_AccessibilitySetting.tiltDisabled);

        Debug.Log($"Tilt Disabled: {CameraTiltControl_AccessibilitySetting.tiltDisabled}" );
    }

    private void OnEnable()
    {
        UpdateUserInterfaceElements(CameraTiltControl_AccessibilitySetting.tiltDisabled);

        CameraTiltControl_AccessibilitySetting.OnCameraTiltControlStateChanged += UpdateUserInterfaceElements;
    }

    private void OnDisable()
    {
        CameraTiltControl_AccessibilitySetting.OnCameraTiltControlStateChanged -= UpdateUserInterfaceElements;
    }

    private void UpdateUserInterfaceElements(bool tiltDisabled)
    {
        if (updateUserInterface == false)
        {
            return;
        }

        imageRenderer.sprite = tiltDisabled == true ? tiltOffSprite : tiltOnSprite;
    }
}
