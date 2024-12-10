using System;
using UnityEngine;

public class OxygenPuzzle : BasePuzzle
{
    [SerializeField] private PuzzleKey requiredActivationKey;

    [SerializeField] private GameObject waterDripObject;
    [SerializeField] private GameObject protonController;
    [SerializeField] private MitoObjectiveSolved mitoObjectiveSolved;

    [SerializeField] private GameObject oxygenPuzzleNavigationObject;
    [SerializeField] private GameObject oxygenPuzzleNavigationLink;

    [SerializeField] private ChamberPower chamberPower;
    [SerializeField] private OxygenPuzzleMolecules oxygenPuzzleMolecules;
    [SerializeField] private BondBreaker bondBreaker;
    [SerializeField] private ProtonReceiver protonReceiver;
    [SerializeField] private OxyProtonSpawner oxyProtonSpawner;

    [SerializeField] private float animationLockOut = 1f;

    [SerializeField] protected EventSequence puzzleCompleteSequence;

    private int protonsDelivered = 0;
    private int protonsRequired = 4;

    public override void NavigateToPuzzle()
    {
        base.NavigateToPuzzle();
    }

    public override void ActivatePuzzle()
    { }

    public override void DeactivatePuzzle()
    { }

    public override void InitializePuzzle_Awake()
    {
        oxygenPuzzleNavigationObject.SetActive(false);
        oxygenPuzzleNavigationLink.SetActive(false);

        oxyProtonSpawner.InitializePool();
        oxyProtonSpawner.gameObject.SetActive(false);

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey))
        {
            return;
        }

        DisablePuzzlePointerHandler();
        BasePuzzle.OnPuzzleCompleted -= EnablePuzzleInteractions;
        BasePuzzle.OnPuzzleCompleted += EnablePuzzleInteractions;

        chamberPower.OnPowerChange -= OnFirstPowerChange;
        chamberPower.OnPowerChange += OnFirstPowerChange;
    }

    public override void InitializePuzzle_Start()
    {
        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey))
        {
            SetToSolvedState();
            return;
        }

        bondBreaker.DisableInteraction();
    }

    private void SetToSolvedState()
    {
        oxyProtonSpawner.gameObject.SetActive(true);
        chamberPower.ChangePower(PowerState.Half);
        oxygenPuzzleMolecules.JumpToLoop();
        PuzzleComplete();
    }

    private void PuzzleComplete()
    {
        chamberPower.SetSound(false);

        InvokeOnPuzzleCompleted();

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey) == false)
        {
            PlayerPrefs_Utilities.SetPuzzleSaveState(puzzleKey, true);
        }

        ProtonDensity._instance.oxy = true;
        waterDripObject.SetActive(true);
        oxyProtonSpawner.DisableObjectInteractions();
        protonController.SetActive(true);

        oxygenPuzzleMolecules.StartLoop();
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

        oxygenPuzzleNavigationObject.SetActive(true);
        oxygenPuzzleNavigationLink.SetActive(true);
        EnablePuzzlePointerHandler();

        oxyProtonSpawner.gameObject.SetActive(true);
        chamberPower.ChangePower(PowerState.Full);
        chamberPower.SetSound(true);
    }

    private void OnFirstPowerChange(PowerState powerState)
    {
        if (powerState == PowerState.Full)
        {
            chamberPower.OnPowerChange -= OnFirstPowerChange;
            oxygenPuzzleMolecules.NextAnimationState();
            Invoke(nameof(EnableBondBreaker), animationLockOut);
        }
    }

    private void EnableBondBreaker()
    {
        bondBreaker.EnableInteraction();
        bondBreaker.OnBreakBond -= OnBreakBond;
        bondBreaker.OnBreakBond += OnBreakBond;
    }

    private void OnBreakBond()
    {
        bondBreaker.OnBreakBond -= OnBreakBond;
        oxygenPuzzleMolecules.NextAnimationState();
        chamberPower.ChangePower(-1);
        bondBreaker.DisableInteraction();

        Invoke(nameof(EnableProtonReceiver), animationLockOut);
    }

    private void EnableProtonReceiver()
    {
        if (chamberPower.PowerState == PowerState.Full)
        {
            protonReceiver.SetActive();
        }
        else
        {
            chamberPower.OnPowerChange -= OnPowerChange;
            chamberPower.OnPowerChange += OnPowerChange;
        }

        protonReceiver.OnProtonReceived -= OnProtonReceived;
        protonReceiver.OnProtonReceived += OnProtonReceived;
    }

    private void OnProtonReceived()
    {
        protonReceiver.SetInactive();
        chamberPower.ChangePower(-1);

        protonsDelivered += 1;

        oxygenPuzzleMolecules.NextAnimationState();

        if (protonsDelivered >= protonsRequired)
        {
            chamberPower.OnPowerChange -= OnPowerChange;
            protonReceiver.OnProtonReceived -= OnProtonReceived;
            PuzzleComplete();
        }
        else
        {
            Invoke(nameof(EnableProtonReceiver), animationLockOut);
        }
    }

    private void OnPowerChange(PowerState powerState)
    {
        if (powerState == PowerState.Full)
        {
            protonReceiver.SetActive();
            chamberPower.OnPowerChange -= OnPowerChange;
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
        StopAllCoroutines();
        BasePuzzle.OnPuzzleCompleted -= EnablePuzzleInteractions;
        chamberPower.OnPowerChange -= OnFirstPowerChange;
        bondBreaker.OnBreakBond -= OnBreakBond;
        chamberPower.OnPowerChange -= OnPowerChange;
        protonReceiver.OnProtonReceived -= OnProtonReceived;
    }
}
