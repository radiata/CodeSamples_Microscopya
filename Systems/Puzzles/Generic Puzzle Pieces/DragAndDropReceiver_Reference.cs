using UnityEngine;

public class DragAndDropReceiver_Reference : MonoBehaviour
{
    [SerializeField] private GameObject dragAndDropReceiver_GameObject;
    private I_DragAndDropReceiver dragAndDropReceiver;

    public I_DragAndDropReceiver GetDragAndDropReceiver() => dragAndDropReceiver;

    private void Awake()
    {
        dragAndDropReceiver = dragAndDropReceiver_GameObject.GetComponent<I_DragAndDropReceiver>();
    }
}
