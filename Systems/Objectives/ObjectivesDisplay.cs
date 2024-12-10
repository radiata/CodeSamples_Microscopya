using UnityEngine;

public class ObjectivesDisplay : MonoBehaviour
{
    [SerializeField] private MultiLanguage_Text_TextMeshProUGUI translationComponent;
    [SerializeField] private TranslatableText_SO emptyObjectiveText; 

    public TranslatableText_SO CurrentTranslatableText => translationComponent.CurrentTranslatableText;

    public void UpdateObjectiveDisplay(TranslatableText_SO translatableText)
    {
        translationComponent.SetNewTranslatableText(translatableText, true);
    }

    public void ClearObjectiveDisplay()
    {
        UpdateObjectiveDisplay(emptyObjectiveText);
    }
}
