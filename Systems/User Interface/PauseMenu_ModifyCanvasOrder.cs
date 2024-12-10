using UnityEngine;

public class PauseMenu_ModifyCanvasOrder : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private int modifiedSortingOrder;

    private int startingSortingOrder;

    private void OnEnable()
    {
        startingSortingOrder = targetCanvas.sortingOrder;
        targetCanvas.sortingOrder = modifiedSortingOrder;
    }

    private void OnDisable()
    {
        targetCanvas.sortingOrder = startingSortingOrder;
    }
}
