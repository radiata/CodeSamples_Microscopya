using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationExitButton : MonoBehaviour
{
    [SerializeField] CustomizationController customizationController;
    [SerializeField] private SoundEffect onClickSound = SoundEffect.GenericCancel;

    [SerializeField] private float fadeInTime;
    [SerializeField] private Image warningImage;
    [SerializeField] private TextMeshProUGUI warningText;

    private bool warningDisplayed = false;
    private Coroutine warningDisplay;

    public void OnClick()
    {
        AudioController.Instance.PlaySoundEffect(onClickSound, false);

        if (customizationController.UnsavedChanges() == true
            && warningDisplayed == false)
        {
            DisplayWarning();
            return;
        }

        customizationController.RevertChanges();
        SceneLoader.Instance.UnloadScene(SceneID.CustomizationMenu);
    }

    public void ResetWarning()
    {
        warningDisplayed = false;

        if(warningDisplay != null)
        {
            StopCoroutine(warningDisplay);
            warningDisplay = null;
        }

        warningText.color = Color.clear;
        warningImage.color = Color.clear;
    }

    private void DisplayWarning()
    {
        if (warningDisplay == null)
        {
            warningDisplay = StartCoroutine(RevealWarning());
        }
    }

    private IEnumerator RevealWarning()
    {
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            warningText.color = Color.Lerp(Color.clear, Color.black, elapsedTime);
            warningImage.color = Color.Lerp(Color.clear, Color.white, elapsedTime);

            elapsedTime += Time.unscaledDeltaTime / fadeInTime;
            yield return null;
        }

        warningText.color = Color.Lerp(Color.clear, Color.black, 1);
        warningImage.color = Color.Lerp(Color.clear, Color.white, 1);

        warningDisplayed = true;
        warningDisplay = null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
