using UnityEngine;

public class HintRevealer_PointerInteractable : PointerInteractable_Base
{
    [SerializeField] private HintRevealer hintRevealer;

    public override int PriorityValue() => PointerInteractable_References.HintRevealerInteraction;

    public override bool IsClickable() => true;
    public override bool IsDraggable() => false;
    public override bool IsHoldable() => false;
    public override bool IsSwipeable() => false;
    public override bool IsPointerContactStartable() => false;

    public override bool SendPassThrough() => false;
    public override bool ReceivePassThrough() => false;

    public override PointerInteractable_Base HoldStart(Vector3 worldPosition, out bool consumed)
    {
        throw new System.NotImplementedException();
    }

    public override void Holding(Vector3 worldPosition, Vector3 cameraForward)
    {
        throw new System.NotImplementedException();
    }

    public override void HoldEnd(Vector3 worldPosition)
    {
        throw new System.NotImplementedException();
    }

    public override bool Click(Vector3 worldPosition, Vector3 cameraForward)
    {
        hintRevealer.DisplayHint();
        return true;
    }

    public override void Drag()
    {
        throw new System.NotImplementedException();
    }

    public override bool Swipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward)
    {
        throw new System.NotImplementedException();
    }

    public override bool PointerContactStart(Vector3 worldPosition, Vector3 cameraForward)
    {
        throw new System.NotImplementedException();
    }
}
