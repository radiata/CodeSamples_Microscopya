using UnityEngine;

public class GearSortingOrderToggle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int sortingOrderOnDisable;

    private int defaultSortingOrder;

    private void Awake()
    {
        defaultSortingOrder = spriteRenderer.sortingOrder;
    }

    private void OnEnable()
    {
        spriteRenderer.sortingOrder = defaultSortingOrder;
    }

    private void OnDisable()
    {
        spriteRenderer.sortingOrder = sortingOrderOnDisable;
    }
}
