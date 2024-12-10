using UnityEngine;

public class TranslationReceiver : MonoBehaviour, I_DragAndDropReceiver
{
    [SerializeField] private TRNAType receivableType;
    [SerializeField] private MRNAChain mRNAChain;

    [SerializeField] private TRNA receivedTRNA;

    private bool receiverActive = true;

    public delegate void ReceiverUpdateEvent(TRNA receivedTRNA);
    public event ReceiverUpdateEvent OnReceiverUpdated;

    public ItemReceipt ReceiveItem(GameObject gameObject)
    {
        if (receiverActive == false)
        {
            return ItemReceipt.ItemRejected;
        }

        receivedTRNA = gameObject.GetComponent<TRNA>();
        if (receivedTRNA == null)
        {
            return ItemReceipt.ItemRejected;
        }

        if (receivedTRNA.Type_tRNA != receivableType)
        {
            receivedTRNA = null;
            return ItemReceipt.ItemRejected;
        }

        receivedTRNA.SetAnchor(mRNAChain.GetActiveIndexTRNAAnchor());
        OnReceiverUpdated?.Invoke(receivedTRNA);

        return ItemReceipt.ItemReceived_ItemValid;
    }

    public void DisableReceiver()
    {
        receiverActive = false;
    }

    public void EnableReceiver()
    {
        receiverActive = true;
    }

    public void ClearReceiver()
    {
        receivedTRNA = null;
    }

    public void SetReceivableType(TRNAType receivableType)
    {
        this.receivableType = receivableType;
    }
}
