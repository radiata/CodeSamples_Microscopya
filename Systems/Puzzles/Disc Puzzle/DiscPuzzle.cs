using UnityEngine;

public class DiscPuzzle : BasePuzzle
{
    [SerializeField] private PuzzleKey requiredActivationKey;
    [SerializeField] private Disc[] discs;

    [SerializeField] private ElectronDispenser solvedDispenser;
    [SerializeField] private MitoObjectiveSolved mitoObjectiveSolved;
    [SerializeField] private GameObject electronSpawner;
    [SerializeField] private GameObject protonController;

    [SerializeField] protected EventSequence puzzleCompleteSequence;

    public override void ActivatePuzzle()
    { }

    public override void DeactivatePuzzle()
    { }

    public override void InitializePuzzle_Awake()
    {
        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey))
        {
            return;
        }

        DisablePuzzlePointerHandler();
        BasePuzzle.OnPuzzleCompleted -= EnablePuzzleInteractions;
        BasePuzzle.OnPuzzleCompleted += EnablePuzzleInteractions;
    }

    public override void InitializePuzzle_Start()
    {
        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey))
        {
            SetToSolvedState();
            return;
        }

        foreach (Disc disc in discs)
        {
            disc.InitializeDisc(false);

            disc.OnDiscSolved -= UpdatePuzzleState;
            disc.OnDiscSolved += UpdatePuzzleState;
        }

        UpdatePuzzleState();
    }

    private void SetToSolvedState()
    {
        foreach (Disc disc in discs)
        {
            disc.InitializeDisc(true);
        }

        electronSpawner.SetActive(true);

        UpdatePuzzleState();
    }

    private void UpdatePuzzleState()
    {
        
        foreach(Disc disc in discs)
        {
            if (disc.isSolved == false)
            {
                return;
            }
        }

        PuzzleComplete();
    }

    private void PuzzleComplete()
    {
        InvokeOnPuzzleCompleted();

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey) == false)
        {
            PlayerPrefs_Utilities.SetPuzzleSaveState(puzzleKey, true);
        }

        ProtonDensity._instance.discs = true;
        solvedDispenser.SolvedDisc();
        protonController.SetActive(true);

        mitoObjectiveSolved.Solved();
        puzzleCompleteSequence.StartOnCallSequence();
    }

    public void EnablePuzzleInteractions(PuzzleKey puzzleKey)
    {
        if (puzzleKey != requiredActivationKey)
        {
            return;
        }

        BasePuzzle.OnPuzzleCompleted -= EnablePuzzleInteractions;

        EnablePuzzlePointerHandler();

        foreach (Disc disc in discs)
        {
            disc.EnableInteractivity();
        }

        electronSpawner.SetActive(true);
    }

    private void OnDisable()
    {
        BasePuzzle.OnPuzzleCompleted -= EnablePuzzleInteractions;

        foreach (Disc disc in discs)
        {
            disc.OnDiscSolved -= UpdatePuzzleState;
        }
    }
}
