using UnityEngine;

public class TRNACharacterSlot : MonoBehaviour
{
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private CollectableTRNA heldTRNA;
    [SerializeField] private TRNACloud tRNACloud;

    public bool isOccupied => heldTRNA != null;

    public void CaptureTRNA(CollectableTRNA collectable)
    {
        heldTRNA = collectable;
        heldTRNA.FollowCharacterAnchor(anchorPoint);
    }

    public void ReleaseTRNAToPuzzle()
    {
        TRNAPuzzleSlot reservedSlot = tRNACloud.ReserveVacantSlot();
        tRNACloud.AssignCharacterHeldTRNAToSlot(heldTRNA, reservedSlot);
        heldTRNA = null;
    }
}
