using UnityEngine;

public class GearReceiver : MonoBehaviour, I_DragAndDropReceiver
{
    [SerializeField] private Gear anchoredGear;

    [SerializeField] private GearType solutionGear;

    [SerializeField] private GearPuzzle gearPuzzle;

    private bool receiverActive = true;

    public delegate void GearChangedEvent();
    public event GearChangedEvent OnGearChanged;

    public bool isSolved => anchoredGear?.GearType == solutionGear;
    public GearType SolutionGear => solutionGear;

    public ItemReceipt ReceiveItem(GameObject gameObject)
    {
        if (receiverActive == false)
        {
            return ItemReceipt.ItemRejected;
        }

        anchoredGear = gameObject.GetComponent<Gear>();
        if (anchoredGear == null)
        {
            return ItemReceipt.ItemRejected;
        }

        anchoredGear.SetPosition(transform.position);
        anchoredGear.UpdateGearHolder(this);
        OnGearChanged?.Invoke();
        receiverActive = false;

        if (isSolved == false)
        {
            return ItemReceipt.ItemReceived_ItemInvalid;
        }
        else
        {
            return gearPuzzle.PriorGearReceiversSolved(this) == true ?
            ItemReceipt.ItemReceived_ItemValid : ItemReceipt.ItemReceived_ItemInvalid;
        }
    }

    public void StartGearRotation()
    {
        anchoredGear.StartGearRotation();
    }

    public void StopGearRotation()
    {
        anchoredGear?.StopGearRotation();
    }

    public void ClearAnchoredGear()
    {
        anchoredGear = null;
        receiverActive = true;
        OnGearChanged?.Invoke();
    }
}
