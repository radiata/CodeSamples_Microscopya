using UnityEngine;

public class BondBreaker : MonoBehaviour, I_ClickResponder
{
    [SerializeField] private Collider2D collider2D;

    public delegate void BreakBondEvent();
    public event BreakBondEvent OnBreakBond;

    public bool OnClick(Vector3 worldPosition)
    {
        OnBreakBond?.Invoke();
        return true;
    }

    public void EnableInteraction()
    {
        collider2D.gameObject.layer = LayerReferences.InteractablePuzzleObjectsLayer;
        collider2D.enabled = true;
    }

    public void DisableInteraction()
    {
        collider2D.gameObject.layer = LayerReferences.NonInteractableLayer;
        collider2D.enabled = false;
    }
}
