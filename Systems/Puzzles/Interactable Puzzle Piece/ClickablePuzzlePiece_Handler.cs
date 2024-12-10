using UnityEngine;

public class ClickablePuzzlePiece_Handler : PointerInteractable_Base
{
    [SerializeField] private GameObject clickablePuzzlePiece_GameObject;
    [SerializeField] private bool isBackgroundPuzzlePiece = false;
    private I_ClickablePuzzlePiece clickablePuzzlePiece;

    public override int PriorityValue() =>
        isBackgroundPuzzlePiece ? PointerInteractable_References.BackgroundPuzzlePieceInteraction : PointerInteractable_References.PuzzlePieceInteraction;

    public override bool IsClickable() => true;
    public override bool IsDraggable() => false;
    public override bool IsHoldable() => false;
    public override bool IsSwipeable() => false;
    public override bool IsPointerContactStartable() => false;

    public override bool SendPassThrough() => false;
    public override bool ReceivePassThrough() => false;

    public static ClickablePuzzlePiece_Handler AddClickablePuzzlePiece_Handler(GameObject targetObject, GameObject clickableObject, bool isBackgroundPuzzlePiece)
    {
        ClickablePuzzlePiece_Handler handler = targetObject.AddComponent<ClickablePuzzlePiece_Handler>();
        handler.clickablePuzzlePiece_GameObject = clickableObject;
        handler.isBackgroundPuzzlePiece = isBackgroundPuzzlePiece;

        handler.Initialize();
        return handler;
    }

    public void SetClickablePuzzlePiece(GameObject gameObject)
    {
        clickablePuzzlePiece_GameObject = gameObject;
    }

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
        clickablePuzzlePiece.OnClick(worldPosition);
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

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        clickablePuzzlePiece = clickablePuzzlePiece_GameObject?.GetComponent<I_ClickablePuzzlePiece>();

        if (clickablePuzzlePiece == null)
        {
            DebugWrapper.Log("Initialization failed", gameObject);
        }
    }
}
