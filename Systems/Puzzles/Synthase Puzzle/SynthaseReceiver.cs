using UnityEngine;

public class SynthaseReceiver : MonoBehaviour, I_DragAndDropReceiver
{
    [SerializeField] private SynthaseAnchorableObject anchoredPuzzlePiece;

    [SerializeField] private SynthaseAnchorableType solutionType;

    [SerializeField] private Transform anchorTransform;

    private bool receiverActive = true;

    public delegate void ReceiverUpdateEvent();
    public event ReceiverUpdateEvent OnReceiverUpdated;

    public bool isSolved => anchoredPuzzlePiece?.SynthaseAnchorableType == solutionType;

    public ItemReceipt ReceiveItem(GameObject gameObject)
    {
        if (receiverActive == false)
        {
            return ItemReceipt.ItemRejected;
        }

        anchoredPuzzlePiece = gameObject.GetComponent<SynthaseAnchorableObject>();
        if (anchoredPuzzlePiece == null)
        {
            return ItemReceipt.ItemRejected;
        }

        anchoredPuzzlePiece.SetAnchor(anchorTransform);
        anchoredPuzzlePiece.UpdateSynthaseReceiver(this);
        OnReceiverUpdated?.Invoke();
        receiverActive = false;

        return ItemReceipt.ItemReceived_ItemValid;
    }

    public void ClearAnchoredPuzzlePiece()
    {
        anchoredPuzzlePiece = null;
        receiverActive = true;
        OnReceiverUpdated?.Invoke();
    }

    public void DestroyAnchoredPuzzlePiece()
    {
        anchoredPuzzlePiece?.DestroySelf();
    }

    public void SetReceiverInactive()
    {
        receiverActive = false;
    }

}
