using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRNAChainAssembler : MonoBehaviour
{
    [SerializeField] private List<Animator> AssembleAnimators;

    [SerializeField] private string assembleTrigger = "AssembleTrigger";
    [SerializeField] private float assembleTime = 1.75f;

    [SerializeField] private string disassembleTrigger = "DisassembleTrigger";
    [SerializeField] private float disassembleTime = 1.75f;

    public delegate void MRNAChainDisassembleCompleteEvent();
    public event MRNAChainDisassembleCompleteEvent OnMRNAChainDisassembled;

    public delegate void MRNAChainAssembleCompleteEvent();
    public event MRNAChainAssembleCompleteEvent OnMRNAChainAssembled;

    public void DisassembleChain()
    {
        StartCoroutine(Disassemble());
    }

    private IEnumerator Disassemble()
    {
        float elapsedTime = 0;

        foreach (var animator in AssembleAnimators)
        {
            animator.SetTrigger(disassembleTrigger);
        }

        while (elapsedTime < disassembleTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        OnMRNAChainDisassembled?.Invoke();
    }

    public void AssembleChain()
    {
        StartCoroutine(Assemble());
    }

    private IEnumerator Assemble()
    {
        float elapsedTime = 0;

        foreach (var animator in AssembleAnimators)
        {
            animator.SetTrigger(assembleTrigger);
        }

        while (elapsedTime < assembleTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        OnMRNAChainAssembled?.Invoke();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
