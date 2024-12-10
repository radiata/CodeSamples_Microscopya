using UnityEngine;

public class SortingOverrideArea2D : MonoBehaviour
{
    [SerializeField] private int sortingOrder;
    [SerializeField] private Collider2D bounds;

    public int GetSortingOrder() => sortingOrder;

    public bool IsInOverrideArea(Vector3 worldPosition)
    {
        return bounds.OverlapPoint(worldPosition);
    }
}
