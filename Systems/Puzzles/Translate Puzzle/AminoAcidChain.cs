using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AminoAcidChain : MonoBehaviour
{
    [SerializeField] private Transform anchorPosition;
    [SerializeField] private List<AminoAcid> aminoAcids;
    [SerializeField] private List<Transform> aminoAcidAnchors;

    private Coroutine progressRoutine;
    private bool busy = false;
    public bool isBusy => busy;

    public delegate void AminoAcidChainUpdateEvent();
    public event AminoAcidChainUpdateEvent OnAminoAcidChainUpdated;


    public void ProgressAminoAcidChain(AminoAcid aminoAcid, Transform nextTRNA)
    {
        if(isBusy)
        {
            return; 
        }

        AddAminoAcid(aminoAcid);
        Reparent(nextTRNA);
    }

    public void AminoAcidChainUpdated()
    {
        OnAminoAcidChainUpdated?.Invoke();
    }

    public void Reparent(Transform newParent, bool changePosition = true)
    {
        transform.position = changePosition ? anchorPosition.position : transform.position;
        transform.SetParent(newParent, true);
        transform.localRotation = Quaternion.identity;
    }

    private void AddAminoAcid(AminoAcid aminoAcid)
    {
        aminoAcids.Insert(0, aminoAcid);
        aminoAcid.transform.SetParent(transform);

        for (int i = 0; i < aminoAcids.Count; i++)
        {
            aminoAcids[i].transform.position = aminoAcidAnchors[i].transform.position;
        }
    }
}