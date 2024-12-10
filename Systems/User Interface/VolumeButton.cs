using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeButton : MonoBehaviour
{
    [SerializeField] private Image buttonImageRenderer;
    [SerializeField] private Sprite mutedSprite;
    [SerializeField] private Sprite unmutedSprite;

    [SerializeField] private GameObject volumeSlider;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    [SerializeField] private GameObject raycastSearchTarget;

    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericConfirm;

    private PointerEventData cachedPointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);

        buttonImageRenderer.enabled = false;
        volumeSlider.SetActive(true);

        InputHandler.OnPointerContactOccurred -= OnPointerContactOccurred;
        InputHandler.OnPointerContactOccurred += OnPointerContactOccurred;
    }

    public void ClearClick()
    {
        buttonImageRenderer.enabled = true;
        volumeSlider.SetActive(false);
        UpdateUserInterfaceElements(AudioVolume.Instance.IsMuted);
    }

    private void OnPointerContactOccurred(Vector2 pointerScreenPosition)
    {
        cachedPointerEventData.position = pointerScreenPosition;
        raycastResults.Clear();

        graphicRaycaster.Raycast(cachedPointerEventData, raycastResults);


        for(int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject == raycastSearchTarget)
            {
                return;
            }
        }

        InputHandler.OnPointerContactOccurred -= OnPointerContactOccurred;
        StartCoroutine(ClearClickAtEndOfFrame());
    }

    private void OnEnable()
    {
        cachedPointerEventData = new PointerEventData(EventSystem.current);

        buttonImageRenderer.enabled = true;
        volumeSlider.SetActive(false);
        UpdateUserInterfaceElements(AudioVolume.Instance.IsMuted);
        AudioVolume.OnMuteChanged += UpdateUserInterfaceElements;
    }

    private void OnDisable()
    {
        AudioVolume.OnMuteChanged -= UpdateUserInterfaceElements;
        InputHandler.OnPointerContactOccurred -= OnPointerContactOccurred;

        StopAllCoroutines();
        ClearClick();
    }

    private void UpdateUserInterfaceElements(bool muted)
    {
        buttonImageRenderer.sprite =
            (muted == true || AudioVolume.Instance.MasterVolume <= 0) ? mutedSprite : unmutedSprite;
    }

    private IEnumerator ClearClickAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        ClearClick();
    }
}
