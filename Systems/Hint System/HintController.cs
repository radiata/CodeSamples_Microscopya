using UnityEngine;

public class HintController : MonoBehaviour
{
    [SerializeField] private GameObject hintContainer;

    private void OnEnable()
    {
        OnHintStateChanged(HintState.HintsEnabledState);
        HintState.OnHintStateChanged += OnHintStateChanged;
    }

    private void OnDisable()
    {
        HintState.OnHintStateChanged -= OnHintStateChanged;
    }

    private void OnHintStateChanged(bool enabled)
    {
        hintContainer.SetActive(enabled);
    }
}
