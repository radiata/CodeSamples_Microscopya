using System.Collections;
using UnityEngine;

public class TranslatePuzzle : BasePuzzle
{
    [Tooltip("The amount of pieces the player needs to place for the puzzle to be considered solved.")]
    [SerializeField] private int manualPlacementsTRNA = 8;
    private int placedTRNA;

    [SerializeField] private TranslationPuzzleAutomation translationPuzzleAutomation;

    [SerializeField] private TranslationReceiver translationReceiver;
    [SerializeField] private AminoAcidChain aminoAcidChain;
    [SerializeField] private MRNAChain mRNAChain;

    [SerializeField] private TRNA initialTRNA;
    [SerializeField] private RibosomeAnimator ribosomeAnimator;

    [SerializeField] private TRNACloud tRNACloud;

    [SerializeField] private GameObject collectableTRNAObject;
    [SerializeField] private CharacterTRNAHolder characterTRNAHolder;

    [SerializeField] private float messageDisplayTime = 8f;

    [SerializeField] private TranslatableText_SO message_Puzzle;
    [SerializeField] private TranslatableText_SO message_FirstCollected;
    [SerializeField] private TranslatableText_SO message_SecondCollected;
    [SerializeField] private TranslatableText_SO message_ThirdCollected;

    private TRNA primedTRNA;
    private TRNA nextTRNA;

    private bool receiveCharacterHeldPieces = false;

    public TranslationReceiver TranslationReceiver => translationReceiver;
    public AminoAcidChain AminoAcidChain => aminoAcidChain;
    public PuzzleManager PuzzleManager => puzzleManager;

    private bool IsSolutionAvailable =>
    tRNACloud.AllSlotsVacant() == true
            ? false : tRNACloud.DoesTRNASolutionAlreadyExist(mRNAChain.GetActiveMRNASet().GetCorrespondingTRNAType());

    private Coroutine onEntrySolvableCheck = null;

    public override void ActivatePuzzle()
    {
        ribosomeAnimator.FadeOut();

        if (receiveCharacterHeldPieces == true)
        {
            characterTRNAHolder.ReleaseToPuzzle();

            if(onEntrySolvableCheck != null)
            {
                StopCoroutine(onEntrySolvableCheck);
            }

            onEntrySolvableCheck = StartCoroutine(OnEntrySolvableCheck());
        }
    }

    public override void DeactivatePuzzle()
    {
        ribosomeAnimator.FadeIn();

        if (receiveCharacterHeldPieces == true)
        {
            SpawnSpeechBubble(0);
        }
    }

    public override void InitializePuzzle_Awake()
    {
        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey))
        {
            return;
        }
    }

    public override void InitializePuzzle_Start()
    {
        mRNAChain.Initialize();

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey))
        {
            SetToSolvedState();
            return;
        }

        translationReceiver.SetReceivableType(mRNAChain.GetActiveMRNASet().GetCorrespondingTRNAType());
        StartCoroutine(DelayedInitialization(1));

        characterTRNAHolder.AttachToCharacter();
    }

    private IEnumerator DelayedInitialization(int frameDelay)
    {
        while (frameDelay > 0)
        {
            yield return null;
            frameDelay--;
        }

        HandleInitialTRNAPriming();

        translationReceiver.OnReceiverUpdated -= OnReceiverUpdated;
        translationReceiver.OnReceiverUpdated += OnReceiverUpdated;

        aminoAcidChain.OnAminoAcidChainUpdated -= OnAminoAcidChainUpdated;
        aminoAcidChain.OnAminoAcidChainUpdated += OnAminoAcidChainUpdated;

        mRNAChain.OnMRNAChainShiftCompleted -= OnMRNAChainShiftCompleted;
        mRNAChain.OnMRNAChainShiftCompleted += OnMRNAChainShiftCompleted;
    }

    private void SetToSolvedState()
    {
        PuzzleComplete();
    }

    private void SpawnNextSetTRNA()
    {
        TRNAType solutionType = mRNAChain.GetActiveMRNASet().GetCorrespondingTRNAType();

        switch (placedTRNA)
        {
            case 1:
                tRNACloud.SpawnTRNA(solutionType, 2);
                break;
            case 2:
                tRNACloud.SpawnTRNA(solutionType, 2);
                break;
            case 3:
                tRNACloud.SpawnTRNA(solutionType, 2);
                break;
            case 4:
                tRNACloud.SpawnTRNA(solutionType, 2);
                break;
            case > 8:
                if (receiveCharacterHeldPieces == false)
                {
                    receiveCharacterHeldPieces = true;
                    ObjectiveUpdaterEvents.ObjectiveCompleted(ObjectiveID_ER.Translation_CheckPoint);

                    if (AllCollected() == true)
                    {
                        characterTRNAHolder.ReleaseToPuzzle();
                    }
                    else
                    {
                        DisablePuzzlePointerHandler();
                    }
                }
                break;
        }
    }

    private void SpawnSpeechBubble(int index)
    {

        if (AllCollected() == true && index == 0)
        {
            return;
        }

        TranslatableText_SO displayText = null;

        switch (index)
        {

            case 0:
                displayText = message_Puzzle;
                break;
            case 1:
                displayText = message_FirstCollected;
                break;
            case 2:
                displayText = message_SecondCollected;
                break;
            case 3:
                displayText = message_ThirdCollected;
                break;
        }

        Message.Instance.DisplayMessage(displayText, messageDisplayTime);
    }

    private int collected = 0;
    public void CollectedPuzzlePiece()
    {
        collected++;

        if(collected >= 3)
        {
            EnablePuzzlePointerHandler();
        }

        if (receiveCharacterHeldPieces == true)
        {
            SpawnSpeechBubble(collected);
        }
    }

    private bool AllCollected()
    {
        return collected >= 3;
    }

    private void OnReceiverUpdated(TRNA receivedTRNA)
    {
        translationReceiver.DisableReceiver();
        nextTRNA = receivedTRNA;

        primedTRNA.PlayReleaseAminoAcidAnimation(nextTRNA.AminoAcidAnchor);
    }

    private void OnAminoAcidChainUpdated()
    {
        mRNAChain.ShiftChainBy(1);

    }

    private void OnMRNAChainShiftCompleted()
    {
        primedTRNA?.ReleaseTRNA(tRNACloud.transform);


        primedTRNA = nextTRNA;
        nextTRNA = null;

        translationReceiver.ClearReceiver();
        translationReceiver.SetReceivableType(mRNAChain.GetActiveMRNASet().GetCorrespondingTRNAType());
        translationReceiver.EnableReceiver();

        UpdatePuzzleState();
    }

    private void UpdatePuzzleState()
    {
        placedTRNA += 1;
        if (placedTRNA >= manualPlacementsTRNA)
        {
            PuzzleComplete();
        }
        else
        {
            SpawnNextSetTRNA();                

            if (IsSolutionAvailable == false)
            {
                puzzleManager.ExitPuzzle();
            }
        }
    }


    private void PuzzleComplete()
    {
        receiveCharacterHeldPieces = false;

        ObjectiveUpdaterEvents.ObjectiveCompleted(ObjectiveID_ER.Translation_Solved);

        InvokeOnPuzzleCompleted();

        if (PlayerPrefs_Utilities.GetPuzzleSaveState(puzzleKey) == false)
        {
            PlayerPrefs_Utilities.SetPuzzleSaveState(puzzleKey, true);
        }

        translationReceiver.OnReceiverUpdated -= OnReceiverUpdated;
        aminoAcidChain.OnAminoAcidChainUpdated -= OnAminoAcidChainUpdated;
        mRNAChain.OnMRNAChainShiftCompleted -= OnMRNAChainShiftCompleted;


        Destroy(collectableTRNAObject);
        characterTRNAHolder.PuzzleCompleted();

        if (initialTRNA != null)
        {
            mRNAChain.SetChainToNextIndex();
            primedTRNA = initialTRNA;
        }

        translationPuzzleAutomation.StartAutomation();
    }

    private void HandleInitialTRNAPriming()
    {
        var dd = initialTRNA.GetComponent<DragAndDrop>();
        dd.SimulateDragAndDrop(translationReceiver.transform.position);
        mRNAChain.SetChainToNextIndex();
        primedTRNA = initialTRNA;

        translationReceiver.ClearReceiver();
        translationReceiver.SetReceivableType(mRNAChain.GetActiveMRNASet().GetCorrespondingTRNAType());
        translationReceiver.EnableReceiver();

        UpdatePuzzleState();
    }

    private void OnDisable()
    {
        translationReceiver.OnReceiverUpdated -= OnReceiverUpdated;
        aminoAcidChain.OnAminoAcidChainUpdated -= OnAminoAcidChainUpdated;
        mRNAChain.OnMRNAChainShiftCompleted -= OnMRNAChainShiftCompleted;

        CancelInvoke();
        StopAllCoroutines();
    }

    public void ReleaseHeldTRNA()
    {
        primedTRNA?.ReleaseTRNA(tRNACloud.transform);
        primedTRNA?.HideAminoAcid();
        nextTRNA?.ReleaseTRNA(tRNACloud.transform);
        nextTRNA?.HideAminoAcid();
    }

    private IEnumerator OnEntrySolvableCheck()
    {
        yield return null;

        if(puzzleManager.PuzzleActive == true
            && IsSolutionAvailable == false)
        {
            puzzleManager.ExitPuzzle();
        }

        onEntrySolvableCheck = null;
    }
}
