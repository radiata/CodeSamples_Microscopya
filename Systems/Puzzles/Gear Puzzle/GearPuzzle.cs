using System.Collections.Generic;
using UnityEngine;

public class GearPuzzle : BasePuzzle
{
    [SerializeField] private List<Gear> gears;
    [SerializeField] private List<GearReceiver> receivers;

    [SerializeField] private ElectronDispenser solvedDispenser;
    [SerializeField] private MitoObjectiveSolved mitoObjectiveSolved;
    [SerializeField] private GearRotation[] enableOnSolved_GearRotations;
    [SerializeField] private newChainController[] enableOnSolved_chainControllers;

    [SerializeField] private GameObject gearsLocationBasedSound;
    [SerializeField] protected EventSequence puzzleCompleteSequence;
    public override void ActivatePuzzle()
    { }

    public override void DeactivatePuzzle()
    { }

    public override void InitializePuzzle_Awake()
    { }

    public override void InitializePuzzle_Start()
    {
        foreach (Gear gear in gears)
        {
            gear.ResetGearPosition();
        }

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey))
        {
            SetToSolvedState();
            return;
        }

        foreach (GearReceiver receiver in receivers)
        {
            receiver.OnGearChanged -= UpdatePuzzleState;
            receiver.OnGearChanged += UpdatePuzzleState;
        }
    }

    private void SetToSolvedState()
    {
        foreach (Gear gear in gears)
        {
            for (int i = 0; i < receivers.Count; i++)
            {
                if (receivers[i].isSolved == false
                    && receivers[i].SolutionGear == gear.GearType)
                {
                    receivers[i].ReceiveItem(gear.gameObject);
                    break;
                }
            }
        }

        UpdatePuzzleState();
    }

    public void UpdatePuzzleState()
    {
        bool priorGearsSolved = true;

        for (int i = 0; i < receivers.Count; i++)
        {
            if (receivers[i].isSolved == true && priorGearsSolved == true)
            {
                receivers[i].StartGearRotation();
            }
            else
            {
                priorGearsSolved = false;
                receivers[i].StopGearRotation();
            }
        }

        if (priorGearsSolved == true)
        {
            PuzzleComplete();
        }
    }

    private void PuzzleComplete()
    {
        foreach (Gear gear in gears)
        {
            gear.RemoveInteractivity();
        }

        InvokeOnPuzzleCompleted();

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey) == false)
        {
            PlayerPrefs_Utilities.SetPuzzleSaveState(puzzleKey, true);
        }

        solvedDispenser.SolvedGear();
        mitoObjectiveSolved.Solved();

        foreach (newChainController chainController in enableOnSolved_chainControllers)
        {
            chainController.enabled = true;
        }

        foreach (GearRotation gearRotation in enableOnSolved_GearRotations)
        {
            gearRotation.enabled = true;
        }

        ProtonDensity._instance.gears = true;
        puzzleCompleteSequence.StartOnCallSequence();
        gearsLocationBasedSound.SetActive(true);
    }

    private void OnDisable()
    {
        foreach (GearReceiver receiver in receivers)
        {
            receiver.OnGearChanged -= UpdatePuzzleState;
        }
    }

    public bool PriorGearReceiversSolved(GearReceiver gearReceiver)
    {
        for (int i = 0; i < receivers.Count; i++)
        {
            if (receivers[i] == gearReceiver)
            {
                return true;
            }

            if (receivers[i].isSolved == false)
            {
                return false;
            }
        }

        return false;
    }
}
