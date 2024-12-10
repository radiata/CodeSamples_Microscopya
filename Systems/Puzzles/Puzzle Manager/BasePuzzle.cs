using UnityEngine;

public abstract class BasePuzzle : MonoBehaviour
{
    [SerializeField] protected PuzzleKey puzzleKey;
    [SerializeField] protected GameObject puzzleManager_PointerInteractable_GameObject;
    [SerializeField] protected PuzzleManager puzzleManager;

    public delegate void PuzzleCompleted(PuzzleKey puzzleKey);
    public static event PuzzleCompleted OnPuzzleCompleted;

    public virtual bool NavigateAvailable => true;
    public PuzzleKey PuzzleKey => puzzleKey;

    //called by Puzzle Manager on Awake
    public abstract void InitializePuzzle_Awake();
    //called by Puzzle Manager on Start
    public abstract void InitializePuzzle_Start();

    //Called by Puzzle Manager on Enter Puzzle
    public abstract void ActivatePuzzle();

    //Called by Puzzle Manager on Exit Puzzle
    public abstract void DeactivatePuzzle();

    public virtual void NavigateToPuzzle()
    {
        puzzleManager.Navigate();
    }

    public void EnablePuzzlePointerHandler()
    {
        puzzleManager_PointerInteractable_GameObject.SetActive(true);
    }

    public void DisablePuzzlePointerHandler()
    {
        puzzleManager_PointerInteractable_GameObject.SetActive(false);
    }

    protected void InvokeOnPuzzleCompleted()
    {
        var delegates = OnPuzzleCompleted?.GetInvocationList();
        OnPuzzleCompleted?.Invoke(puzzleKey);
    }
}
