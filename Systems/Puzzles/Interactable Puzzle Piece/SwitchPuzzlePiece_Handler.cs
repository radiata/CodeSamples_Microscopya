using UnityEngine;

public class SwitchPuzzlePiece_Handler : PointerInteractable_Base
{
    [SerializeField] private GameObject switchPuzzlePiece_GameObject;
    [SerializeField] private bool isBackgroundPuzzlePiece = false;
    private I_SwitchPuzzlePiece switchPuzzlePiece;

    public override int PriorityValue() =>
        isBackgroundPuzzlePiece ? PointerInteractable_References.BackgroundPuzzlePieceInteraction : PointerInteractable_References.PuzzlePieceInteraction;

    public override bool IsClickable() => true;
    public override bool IsDraggable() => false;
    public override bool IsHoldable() => true;
    public override bool IsSwipeable() => true;
    public override bool IsPointerContactStartable() => false;

    public override bool SendPassThrough() => false;
    public override bool ReceivePassThrough() => false;

    public static SwitchPuzzlePiece_Handler AddSwitchPuzzlePiece_Handler(GameObject targetObject, GameObject switchObject, bool isBackgroundPuzzlePiece)
    {
        SwitchPuzzlePiece_Handler handler = targetObject.AddComponent<SwitchPuzzlePiece_Handler>();
        handler.switchPuzzlePiece_GameObject = switchObject;
        handler.isBackgroundPuzzlePiece = isBackgroundPuzzlePiece;

        handler.Initialize();
        return handler;
    }

    public void SetSwitchPuzzlePiece(GameObject gameObject)
    {
        switchPuzzlePiece_GameObject = gameObject;
    }

    public override PointerInteractable_Base HoldStart(Vector3 worldPosition, out bool consumed)
    {
        switchPuzzlePiece.OnDragStart(worldPosition);
        consumed = true;
        return this;
    }

    public override void Holding(Vector3 worldPosition, Vector3 cameraForward)
    {
        switchPuzzlePiece.WhileDragging(worldPosition, cameraForward);
    }

    public override void HoldEnd(Vector3 worldPosition)
    {
        switchPuzzlePiece.OnDragEnd(worldPosition);
    }

    public override bool Click(Vector3 worldPosition, Vector3 cameraForward)
    {
        switchPuzzlePiece.SwitchToggle();
        return true;
    }

    public override bool Swipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward)
    {
        switchPuzzlePiece.SwitchToggle();
        return true;
    }

    public override void Drag()
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
        switchPuzzlePiece = switchPuzzlePiece_GameObject?.GetComponent<I_SwitchPuzzlePiece>();

        if (switchPuzzlePiece == null)
        {
            DebugWrapper.Log("Initialization failed", gameObject);
        }
    }
}
