using UnityEngine;

public class BehaviourChangingPuzzlePiece_Handler : MonoBehaviour
{
    [SerializeField] private StartingState startingState = StartingState.None;
    [SerializeField] private GameObject puzzlePiece_GameObject;
    [SerializeField] private bool isBackgroundPuzzlePiece = false;

    private bool clickable = false;
    private bool draggable = false;
    private bool switchable = false;

    private PointerInteractable_Base activeBehaviour;

    [ContextMenu("Set to Clickable")]
    public void SetClickable()
    {
        if (clickable == true)
        {
            return;
        }

        DisableAll();

        clickable = true;

        ClickablePuzzlePiece_Handler handler =
            ClickablePuzzlePiece_Handler.AddClickablePuzzlePiece_Handler(gameObject, puzzlePiece_GameObject, isBackgroundPuzzlePiece);

        handler.Initialize();
        activeBehaviour = handler;
    }

    [ContextMenu("Set Draggable")]
    public void SetDraggable()
    {
        if (draggable == true)
        {
            return;
        }

        DisableAll();

        draggable = true;

        DraggablePuzzlePiece_Handler handler =
             DraggablePuzzlePiece_Handler.AddDraggablePuzzlePiece_Handler(gameObject, puzzlePiece_GameObject, isBackgroundPuzzlePiece);

        activeBehaviour = handler;
    }

    [ContextMenu("Set Switch")]
    public void SetSwitchable()
    {
        if (switchable == true)
        {
            return;
        }

        DisableAll();

        switchable = true;

        SwitchPuzzlePiece_Handler handler =
            SwitchPuzzlePiece_Handler.AddSwitchPuzzlePiece_Handler(gameObject, puzzlePiece_GameObject, isBackgroundPuzzlePiece);

        handler.Initialize();
        activeBehaviour = handler;
    }

    public void DisableAll()
    {
        clickable = false;
        draggable = false;
        switchable = false;

        if (activeBehaviour != null)
        {
            Destroy(activeBehaviour);
        }
    }

    private void Start()
    {
        switch (startingState)
        {
            case StartingState.None:
                break;
            case StartingState.Clickable:
                SetClickable();
                break;
            case StartingState.Draggable:
                SetDraggable();
                break;
            case StartingState.Switchable:
                SetSwitchable();
                break;
        }
    }

    [System.Serializable]
    private enum StartingState
    {
        None = 0,
        Clickable = 1,
        Draggable = 2,
        Switchable = 3,
    }
}


