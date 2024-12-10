using System.Collections.Generic;
using UnityEngine;

public class PaddlePuzzle : BasePuzzle
{
    [SerializeField] private PuzzleKey requiredActivationKey;
    [SerializeField] private SpriteRevealer puzzleInteriorRevealer;
    [SerializeField] private List<Paddle> paddles;

    [SerializeField] private List<Paddle> initializeAsUnsolved;
    [SerializeField] private List<Paddle> initializeAsSolved;
    [SerializeField] private List<Paddle> initializeAsRandom;

    [SerializeField] private GameObject emitter1, emitter2;
    [SerializeField] private GameObject protonController;
    [SerializeField] private GameObject electronDispenser;
    [SerializeField] private MitoObjectiveSolved mitoObjectiveSolved;

    [SerializeField] protected EventSequence puzzleCompleteSequence;

    [SerializeField] private float fadeDelayOnSolved = 5f;
    private bool useDelayedFade = false;

    public override void ActivatePuzzle()
    {
        puzzleInteriorRevealer.ChangeRevealedState(true);
        CancelInvoke(nameof(DisableEmitters));

        foreach (Paddle paddle in paddles)
        {
            paddle.EnableInteractivity();
        }

        if (emitter1.activeSelf == false)
        {
            emitter1.SetActive(true);
        }

        if (emitter2.activeSelf == false)
        {
            emitter2.SetActive(true);
        }
    }

    public override void DeactivatePuzzle()
    {
        if (useDelayedFade == true)
        {
            Invoke(nameof(PuzzleFade), fadeDelayOnSolved);
        }
        else
        {
            PuzzleFade();
        }

        foreach (Paddle paddle in paddles)
        {
            paddle.DisableInteractivity();
        }
    }

    private void PuzzleFade()
    {
        puzzleInteriorRevealer.ChangeRevealedState(false);
        Invoke(nameof(DisableEmitters), puzzleInteriorRevealer.TransitionTime);
    }

    private void DisableEmitters()
    {
        emitter1.SetActive(false);
        emitter2.SetActive(false);
    }

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

        InitializePaddles();
        UpdatePuzzleState();
    }

    private void InitializePaddles()
    {
        foreach (Paddle paddle in paddles)
        {
            paddle.OnPaddleSwitched -= UpdatePuzzleState;
            paddle.OnPaddleSwitched += UpdatePuzzleState;
        }

        foreach (Paddle paddle in initializeAsUnsolved)
        {
            paddle.InitializePaddle(false);
        }

        foreach (Paddle paddle in initializeAsSolved)
        {
            paddle.InitializePaddle(true);
        }

        foreach (Paddle paddle in initializeAsRandom)
        {
            bool initializeAsSolved = UnityEngine.Random.value >= 0.5;
            paddle.InitializePaddle(initializeAsSolved);
        }
    }

    private void SetToSolvedState()
    {
        foreach (Paddle paddle in paddles)
        {
            paddle.InitializePaddle(true);
        }

        UpdatePuzzleState();
    }

    private void UpdatePuzzleState()
    {
        foreach (Paddle paddle in paddles)
        {
            if (paddle.isSolved() == false)
            {
                return;
            }
        }

        PuzzleComplete();
    }

    private void PuzzleComplete()
    {
        useDelayedFade = true;
        InvokeOnPuzzleCompleted();

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey) == false)
        {
            PlayerPrefs_Utilities.SetPuzzleSaveState(puzzleKey, true);
        }

        ProtonDensity._instance.paddles = true;
        protonController.SetActive(true);
        electronDispenser.SetActive(true);

        mitoObjectiveSolved.Solved();
        puzzleCompleteSequence.StartOnCallSequence();
    }

    private void EnablePuzzleInteractions(PuzzleKey puzzleKey)
    {
        if (puzzleKey != requiredActivationKey)
        {
            return;
        }
        BasePuzzle.OnPuzzleCompleted -= EnablePuzzleInteractions;

        EnablePuzzlePointerHandler();
    }

    private void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();
        BasePuzzle.OnPuzzleCompleted -= EnablePuzzleInteractions;

        foreach (Paddle paddle in paddles)
        {
            paddle.OnPaddleSwitched -= UpdatePuzzleState;
        }
    }
}
