using TMPro;
using UnityEngine;

public class ResearchLabel : MonoBehaviour
{
    [SerializeField] private GameObject label_GameObject;

    [SerializeField] private TextMeshPro textMeshProUGUI;
    [SerializeField] private SpriteRenderer labelSpriteRenderer;

    [SerializeField] private Sprite primaryLabelSprite;
    [SerializeField] private Sprite secondaryLabelSprite;

    [SerializeField] private GameObject interactionArea;

    private string lockedPrimaryText = "???";
    private string lockedSecondaryText = "";

    public void ActivateAsPrimaryLabel(bool isUnlocked, string unlockedText)
    {
        labelSpriteRenderer.sprite = primaryLabelSprite;

        if (isUnlocked == false)
        {
            textMeshProUGUI.text = lockedPrimaryText;
        }
        else
        {
            textMeshProUGUI.text = unlockedText;
        }

        label_GameObject.SetActive(true);
        interactionArea.SetActive(true);
    }

    public void ActivateAsSecondaryLabel(bool isUnlocked, string unlockedText)
    {
        if (isUnlocked == false)
        {
            labelSpriteRenderer.sprite = secondaryLabelSprite;
            textMeshProUGUI.text = lockedSecondaryText;
            interactionArea.SetActive(false);
        }
        else
        {
            labelSpriteRenderer.sprite = primaryLabelSprite;
            textMeshProUGUI.text = unlockedText;
            interactionArea.SetActive(true);
        }

        label_GameObject.SetActive(true);
    }

    public void DeactivateLabel()
    {
        if (label_GameObject == null
            || interactionArea == null)
        {
            return;
        }

        label_GameObject.SetActive(false);
        interactionArea.SetActive(false);
    }

    private void Reset()
    {
        label_GameObject = gameObject;
    }

    public void EDITOR_SetText(string text)
    {
        textMeshProUGUI.text = text;
    }
}
