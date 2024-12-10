using UnityEngine;

public class MotorProteinFootReceiver : MonoBehaviour, I_DragAndDropReceiver
{
    [SerializeField] private MotorProteinPuzzle motorProteinPuzzle;

    [SerializeField] private Transform anchorPosition;

    private AdenosineTriphosphate anchoredATP;
    private ADP anchoredADP;

    private bool receiverActive = false;

    public ItemReceipt ReceiveItem(GameObject gameObject)
    {
        if(receiverActive == false)
        {
            return ItemReceipt.ItemRejected;
        }

        anchoredATP = gameObject.GetComponent<AdenosineTriphosphate>();
        if (anchoredATP == null)
        {
            return ItemReceipt.ItemRejected;
        }

        ReleaseADP();

        anchoredATP.transform.SetParent(anchorPosition);
        anchoredATP.transform.localPosition = Vector3.zero;
        anchoredATP.transform.localRotation = Quaternion.identity;

        motorProteinPuzzle.PerformStep(this);

        return ItemReceipt.ItemReceived_ItemValid;
    }

    public void ReleaseADP()
    {
        if(anchoredADP == null)
        {
            return;
        }

        anchoredADP.Release();
        anchoredADP = null;
    }

    public void ConvertATP()
    {
        if(anchoredATP == null)
        {
            return;
        }

        anchoredADP = anchoredATP.ConvertToADP();
    }

    public void EnableReceiver()
    {
        receiverActive = true;
    }

    public void DisableReceiver()
    {
        receiverActive = false;
    }
}
