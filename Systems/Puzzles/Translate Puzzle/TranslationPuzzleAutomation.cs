using System.Collections;
using UnityEngine;

public class TranslationPuzzleAutomation : MonoBehaviour
{
    [SerializeField] private RibosomeAnimator ribosomeAnimator;

    [SerializeField] private TranslatePuzzle translatePuzzle;
    [SerializeField] private MRNAChain mRNAChain;
    [SerializeField] private MRNAChainAssembler chainAssembler;

    [SerializeField] private AminoAcidChain aminoAcidChain;
    [SerializeField] private Transform aminoAcidParent;

    [SerializeField] private AutomatedTRNA automatedTRNA;

    public delegate void AutomationCompleteEvent();
    public event AutomationCompleteEvent OnAutomationComplete;

    private bool isAutomated = false;
    public bool IsAutomated => isAutomated;

    private float stepBuffer;
    private float defaultDuration = 0;
    private bool firstRun = true;

    public void StartAutomation()
    {
        isAutomated = true;
        ribosomeAnimator.StartAutomation();

        Invoke(nameof(AutomationStep0), ribosomeAnimator.FadeTimer);
        mRNAChain.OnMRNAChainShiftCompleted += AutomationStep2;
        //chainAssembler.OnMRNAChainDisassembled += ResetAutomation;
        chainAssembler.OnMRNAChainAssembled += AutomationStep1;
    }

    public void EndAutomation()
    {
        mRNAChain.SetChainToEnd();
    }

    private void ResetAutomation()
    {
        mRNAChain.ResetPosition();
        mRNAChain.ResetActiveIndex();
        mRNAChain.ResetMRNAChain();
        chainAssembler.AssembleChain();
    }

    private void AutomationStep0()
    {
        aminoAcidChain.Reparent(aminoAcidParent, false);
        
        AutomationStep1();
    }

    private void AutomationStep1()
    {
        StartCoroutine(AutomationStep1Routine());
    }

    private void AutomationStep2()
    {
        if (mRNAChain.ActiveIndex > mRNAChain.EndIndex)
        {
            automatedTRNA.DespawnStopCodon();
            chainAssembler.DisassembleChain();
            OnAutomationComplete?.Invoke();
        }
        else if (mRNAChain.ActiveIndex == mRNAChain.EndIndex)
        {
            automatedTRNA.DespawnOldest();
            AutomationEnd();
        }
        else
        {
            if (firstRun != true)
            {
                automatedTRNA.DespawnOldest();
            }
            else
            {
                translatePuzzle.ReleaseHeldTRNA();
                firstRun = false;
            }
            AutomationStep1();
        }
    }

    private void AutomationEnd()
    {
        StartCoroutine(AutomationEndRoutine());
    }

    private void OnDestroy()
    {
        mRNAChain.OnMRNAChainShiftCompleted -= AutomationStep2;
        //chainAssembler.OnMRNAChainDisassembled -= ResetAutomation;
        chainAssembler.OnMRNAChainAssembled -= AutomationStep1;
        CancelInvoke();
    }

    private IEnumerator AutomationStep1Routine()
    {
        float? duration = automatedTRNA.SpawnSimpleTRNA();
        if (duration.HasValue == false)
        {
            duration = defaultDuration;
        }

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        mRNAChain.ShiftChainBy(1);
    }

    private IEnumerator AutomationEndRoutine()
    {
        float? duration = automatedTRNA.SpawnStopCodon();
        if (duration.HasValue == false)
        {
            duration = defaultDuration;
        }

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        mRNAChain.ShiftChainBy(1);
    }
}
