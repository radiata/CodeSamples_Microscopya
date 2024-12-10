using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraFader : MonoBehaviour
{
    [SerializeField] private Canvas fadeCanvas;
    [SerializeField] private Image fadeImage;

    private FadeParams activeFadeParams;
    private Coroutine activeFadeCoroutine;

    public void StartFade(FadeParams fadeParams)
    {
        if (activeFadeCoroutine != null)
        {
            EndFade();
        }

        activeFadeParams = fadeParams;
        activeFadeCoroutine = StartCoroutine(FadeImageRoutine());
    }

    public void EndFade()
    {
        if (activeFadeCoroutine != null)
        {
            StopCoroutine(activeFadeCoroutine);
            activeFadeCoroutine = null;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, activeFadeParams.endAlpha);

        if (activeFadeParams.endAsDisabled == true)
        {
            fadeCanvas.enabled = false;
        }
    }

    IEnumerator FadeImageRoutine()
    {
        float delta = activeFadeParams.startAlpha - activeFadeParams.endAlpha;
        Color imageColor = fadeImage.color;

        imageColor.a = activeFadeParams.startAlpha;
        fadeImage.color = imageColor;
        yield return null;

        if (fadeCanvas.enabled == false)
        {
            fadeCanvas.enabled = true;
        }

        while (imageColor.a != activeFadeParams.endAlpha)
        {
            imageColor.a = Mathf.Clamp(imageColor.a - (delta * (Time.deltaTime / activeFadeParams.durationSeconds)), activeFadeParams.endAlpha, activeFadeParams.startAlpha);
            fadeImage.color = imageColor;
            yield return null;
        }

        EndFade();
    }
}