using UnityEngine;

public class PuzzleManager_PointerInteractable : PointerInteractable_Base
{
    [SerializeField] private PuzzleManager puzzleManager;

    public override int PriorityValue() => PointerInteractable_References.PuzzleInteraction;

    public override bool IsClickable() => true;
    public override bool IsDraggable() => false;
    public override bool IsHoldable() => true;
    public override bool IsSwipeable() => false;
    public override bool IsPointerContactStartable() => false;

    public override bool SendPassThrough() => false;
    public override bool ReceivePassThrough() => false;

    public void SetPuzzleManager(PuzzleManager puzzleManager)
    {
        this.puzzleManager = puzzleManager;
    }

    public override bool Click(Vector3 worldPosition, Vector3 cameraForward)
    {
        puzzleManager.Navigate();
        return true;
    }

    public override void Drag()
    {
        throw new System.NotImplementedException();
    }

    public override void HoldEnd(Vector3 worldPosition)
    {
        return;
    }

    public override void Holding(Vector3 worldPosition, Vector3 cameraForward)
    {
        InputHandler.Instance.ResetHold(worldPosition);
        return;
    }

    public override PointerInteractable_Base HoldStart(Vector3 worldPosition, out bool consumed)
    {
        puzzleManager.Navigate();
        consumed = true;
        return this;
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
