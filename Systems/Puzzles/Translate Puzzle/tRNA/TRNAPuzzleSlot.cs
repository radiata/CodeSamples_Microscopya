using UnityEngine;

public class TRNAPuzzleSlot : MonoBehaviour
{
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private TRNA heldTRNA;

    private bool isReserved = false;

    public bool isOccupied => (heldTRNA != null || isReserved);
    public TRNAType HeldTRNAType => heldTRNA.Type_tRNA;

    public void ReserveSlot()
    {
        isReserved = true;
    }

    public void ReceiveTRNA(TRNA tRNA)
    {
        heldTRNA = tRNA;
        heldTRNA.FreeFloat.enabled = false;
        heldTRNA.DisableInteractions();
        heldTRNA.transform.SetParent(anchorPoint);

        heldTRNA.FloatInAndFadeIn.OnLerpFromLocationCompleted -= OnFloatInComplete;
        heldTRNA.FloatInAndFadeIn.OnLerpFromLocationCompleted += OnFloatInComplete;
        heldTRNA.FloatInAndFadeIn.StartBehaviour();
    }

    public void ReceiveCharacterHeldTRNA(CollectableTRNA tRNA)
    {
        tRNA.ReleaseFromCharacter();
        heldTRNA = tRNA.TRNA;
        heldTRNA.FreeFloat.enabled = false;
        heldTRNA.DisableInteractions();
        tRNA.SetDraggable();
        heldTRNA.transform.SetParent(anchorPoint);

        heldTRNA.FloatInAndFadeIn.OnLerpFromLocationCompleted -= OnFloatInComplete_FromCharacter;
        heldTRNA.FloatInAndFadeIn.OnLerpFromLocationCompleted += OnFloatInComplete_FromCharacter;
        heldTRNA.FloatInAndFadeIn.StartBehaviour_FromCharacter(heldTRNA.transform.localPosition);
    }

    private void OnFloatInComplete()
    {
        heldTRNA.FloatInAndFadeIn.OnLerpFromLocationCompleted -= OnFloatInComplete;

        heldTRNA.FreeFloat.enabled = true;
        heldTRNA.EnableInteractions();

        heldTRNA.OnAnchorSet -= OnAnchorSet;
        heldTRNA.OnAnchorSet += OnAnchorSet;
    }

    private void OnFloatInComplete_FromCharacter()
    {
        heldTRNA.FloatInAndFadeIn.OnLerpFromLocationCompleted -= OnFloatInComplete_FromCharacter;

        heldTRNA.FreeFloat.enabled = true;
        heldTRNA.EnableInteractions();
        isReserved = false;

        heldTRNA.OnAnchorSet -= OnAnchorSet;
        heldTRNA.OnAnchorSet += OnAnchorSet;
    }

    private void OnAnchorSet()
    {
        heldTRNA.OnAnchorSet -= OnAnchorSet;

        heldTRNA = null;
    }

    private void OnDestroy()
    {
        if(heldTRNA == null)
        {
            return;
        }

        heldTRNA.FloatInAndFadeIn.OnLerpFromLocationCompleted -= OnFloatInComplete;
        heldTRNA.FloatInAndFadeIn.OnLerpFromLocationCompleted -= OnFloatInComplete_FromCharacter;
        heldTRNA.OnAnchorSet -= OnAnchorSet;
    }
}