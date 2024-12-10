using UnityEngine;
using TMPro;

public class SynthasePuzzle : BasePuzzle
{
    [SerializeField] private PuzzleKey requiredActivationKey;

    [SerializeField] private SynthaseWheel synthaseWheel;
    [SerializeField] private SynthaseWheelActivator synthaseWheelActivator;
    [SerializeField] private SynthaseReceiver adpReceiver;
    [SerializeField] private SynthaseReceiver phosphateReceiver;
    [SerializeField] private SynthaseTurbine synthaseTurbine;
    [SerializeField] private SynthasePress synthasePress;

    [SerializeField] private MitoObjectiveSolved mitoObjectiveSolved;
    [SerializeField] private MitoExitController mitoExitController;

    [SerializeField] private EventSequence onPuzzleCompleteSequence;

    private float loopAnimationDelay = 1f;

    public override bool NavigateAvailable => puzzleManager_PointerInteractable_GameObject.activeSelf;

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
        synthaseTurbine.InitializeTurbine();

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey))
        {
            SetToSolvedState();
            return;
        }

        synthaseWheel.OnWheelUpdated -= UpdatePuzzleState;
        synthaseWheel.OnWheelUpdated += UpdatePuzzleState;

        adpReceiver.OnReceiverUpdated -= UpdatePuzzleState;
        adpReceiver.OnReceiverUpdated += UpdatePuzzleState;
        
        phosphateReceiver.OnReceiverUpdated -= UpdatePuzzleState;
        phosphateReceiver.OnReceiverUpdated += UpdatePuzzleState;

        synthaseWheelActivator.EnableInteraction();
    }

    private void SetToSolvedState()
    {
        synthaseTurbine.ActivateTurbine();
        PuzzleComplete();
    }

    private void EnablePuzzleInteractions(PuzzleKey puzzleKey)
    {
        if (puzzleKey != requiredActivationKey)
        {
            return;
        }
        BasePuzzle.OnPuzzleCompleted -= EnablePuzzleInteractions;
        synthaseWheel.UnlockWheel();

        EnablePuzzlePointerHandler();
    }

    private void UpdatePuzzleState()
    {
        if (synthaseWheel.isSolved == false)
        {
            return;
        }
        else
        {
            synthaseTurbine.ActivateTurbine();
        }

        if(adpReceiver.isSolved == false)
        {
            return;
        }

        if(phosphateReceiver.isSolved == false)
        {
            return;
        }

        synthasePress.PlayFirstAnimation();
        PuzzleComplete();
    }

    private void PuzzleComplete()
    {
        adpReceiver.SetReceiverInactive();
        phosphateReceiver.SetReceiverInactive();
        synthaseWheelActivator.DisableInteraction();

        InvokeOnPuzzleCompleted();
        Invoke(nameof(PlaySynthasePressLoop), loopAnimationDelay);

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey) == false)
        {
            PlayerPrefs_Utilities.SetPuzzleSaveState(puzzleKey, true);
        }

        mitoObjectiveSolved.Solved();
        onPuzzleCompleteSequence.StartOnCallSequence();
        ProtonDensity._instance.synth = true;
        mitoExitController.EnableExit();
    }

    private void PlaySynthasePressLoop()
    {
        synthasePress.PlayLoopAnimation();
    }

    private void OnDestroy()
    {
        BasePuzzle.OnPuzzleCompleted -= EnablePuzzleInteractions;
        synthaseWheel.OnWheelUpdated -= UpdatePuzzleState;
        adpReceiver.OnReceiverUpdated -= UpdatePuzzleState;
        phosphateReceiver.OnReceiverUpdated -= UpdatePuzzleState;

        CancelInvoke();
        StopAllCoroutines();
    }
}
