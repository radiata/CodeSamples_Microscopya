using UnityEngine;

public class DraggablePuzzlePiece_Handler : PointerInteractable_Base
{
    [SerializeField] private GameObject draggablePuzzlePiece_GameObject;
    [SerializeField] private bool isBackgroundPuzzlePiece = false;
    [SerializeField] private bool cancelMultitouchDelay = true;

    private I_DraggablePuzzlePiece draggablePuzzlePiece;

    public override int PriorityValue() => 
        isBackgroundPuzzlePiece ? PointerInteractable_References.BackgroundPuzzlePieceInteraction : PointerInteractable_References.PuzzlePieceInteraction;

    public override bool IsClickable() => true;
    public override bool IsDraggable() => false;
    public override bool IsHoldable() => true;
    public override bool IsSwipeable() => true;
    public override bool IsPointerContactStartable() => true;

    public override bool SendPassThrough() => false;
    public override bool ReceivePassThrough() => false;

    public static DraggablePuzzlePiece_Handler AddDraggablePuzzlePiece_Handler(GameObject targetObject, GameObject draggableObject, bool isBackgroundPuzzlePiece)
    {
        DraggablePuzzlePiece_Handler handler = targetObject.AddComponent<DraggablePuzzlePiece_Handler>();
        handler.draggablePuzzlePiece_GameObject = draggableObject;
        handler.isBackgroundPuzzlePiece = isBackgroundPuzzlePiece;

        handler.Initialize();
        return handler;
    }

    public void SetDraggablePuzzlePiece(GameObject gameObject)
    {
        draggablePuzzlePiece_GameObject = gameObject;
    }

    public override PointerInteractable_Base HoldStart(Vector3 worldPosition, out bool consumed)
    {
        draggablePuzzlePiece.OnDragStart(worldPosition);
        consumed = true;
        return this;
    }

    public override void Holding(Vector3 worldPosition, Vector3 cameraForward)
    {
        draggablePuzzlePiece.WhileDragging(worldPosition, cameraForward);
    }

    public override void HoldEnd(Vector3 worldPosition)
    {
        draggablePuzzlePiece.OnDragEnd(worldPosition);
    }

    public override bool Click(Vector3 worldPosition, Vector3 cameraForward)
    {
        draggablePuzzlePiece.OnDragStart(worldPosition);
        draggablePuzzlePiece.OnDragEnd(worldPosition);
        return true;
    }

    public override void Drag()
    {
        throw new System.NotImplementedException();
    }

    public override bool Swipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward)
    {
        draggablePuzzlePiece.OnDragStart(worldStartPosition);
        draggablePuzzlePiece.WhileDragging(worldEndPosition, cameraForward);
        draggablePuzzlePiece.OnDragEnd(worldEndPosition);
        return true;
    }

    public override bool PointerContactStart(Vector3 worldPosition, Vector3 cameraForward)
    {
        if(cancelMultitouchDelay == true)
        {
            InputHandlerEvents.RaiseSkipMultiTouchDelayEvent();
        }
        return true;
    }

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        draggablePuzzlePiece = draggablePuzzlePiece_GameObject?.GetComponent<I_DraggablePuzzlePiece>();

        if(draggablePuzzlePiece == null)
        {
            DebugWrapper.Log("Initialization failed", gameObject);
        }
    }
}
