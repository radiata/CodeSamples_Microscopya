using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MRNAPool : MonoBehaviour
{
    [SerializeField] private MRNA_SO mRNA_SO;

    [SerializeField] private Transform poolParent;

    //bool represents inUse, so false means it is available for use
    [SerializeField] private List<MRNAPoolObject> mRNAPool;

    [SerializeField] private int emptyMRNACount = 3;
    [SerializeField] private List<MRNAPoolObject> emptyMRNAPool;

    private List<(int3, int)> uniqueSets;

    [ContextMenu("Create Pool Objects")]

    public void CreatePool()
    {
        CalculatePoolRequirements();

        for (int i = 0; i < uniqueSets.Count; i++)
        {
            for (int j = 0; j < uniqueSets[i].Item2; j++)
            {
                MRNASet mRNASet = mRNA_SO.GenerateMRNAGameObject(uniqueSets[i].Item1, poolParent);
                mRNAPool.Add(new MRNAPoolObject(mRNASet, false));
                mRNASet.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < emptyMRNACount; i++)
        {
            MRNASet mRNASet = mRNA_SO.GenerateEmptyMRNAGameObject(poolParent);
            emptyMRNAPool.Add(new MRNAPoolObject(mRNASet, false));
            mRNASet.gameObject.SetActive(false);
            mRNASet.OverlayRenderer.sprite = null;
        }
    }

    [ContextMenu("Destroy Pool Objects")]
    public void EmptyPool()
    {
        mRNAPool.Clear();
        emptyMRNAPool.Clear();

        while (poolParent.childCount > 0)
        {
            DestroyImmediate(poolParent.GetChild(0).gameObject);
        }
    }

    public MRNAPoolObject GetNext(int3 sequenceType)
    {
        foreach (var item in mRNAPool)
        {
            if (item.mRNASet.isEquivalentSequence(sequenceType))
            {
                if (item.inUse == false)
                {
                    item.inUse = true;
                    item.mRNASet.gameObject.SetActive(true);
                    return item;
                }
            }
        }

        var poolObject = CreateNewItem(sequenceType);
        poolObject.inUse = true;
        poolObject.mRNASet.gameObject.SetActive(true);
        return poolObject;
    }

    public MRNAPoolObject GetEmpty()
    {
        foreach (var item in emptyMRNAPool)
        {
            if (item.inUse == false)
            {
                item.inUse = true;
                item.mRNASet.gameObject.SetActive(true);
                return item;
            }
        }

        var poolObject = CreateNewEmptyItem();
        poolObject.inUse = true;
        poolObject.mRNASet.gameObject.SetActive(true);
        return poolObject;
    }

    public void Recycle(MRNAPoolObject poolObject)
    {
        poolObject.mRNASet.gameObject.SetActive(false);
        poolObject.mRNASet.transform.SetParent(poolParent);
        poolObject.mRNASet.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        poolObject.mRNASet.transform.localScale = Vector3.one;
        poolObject.inUse = false;
    }

    private MRNAPoolObject CreateNewItem(int3 sequenceType)
    {
        MRNASet mRNASet = mRNA_SO.GenerateMRNAGameObject(sequenceType, poolParent);
        MRNAPoolObject poolObject = new MRNAPoolObject(mRNASet, false);
        mRNAPool.Add(poolObject);
        mRNASet.gameObject.SetActive(false);
        mRNASet.OverlayRenderer.sprite = null;
        return poolObject;
    }

    private MRNAPoolObject CreateNewEmptyItem()
    {
        MRNASet mRNASet = mRNA_SO.GenerateEmptyMRNAGameObject(poolParent);
        MRNAPoolObject poolObject = new MRNAPoolObject(mRNASet, false);
        emptyMRNAPool.Add(poolObject);
        mRNASet.gameObject.SetActive(false);
        return poolObject;
    }

    private void CalculatePoolRequirements()
    {
        uniqueSets = CreateLisOfUniqueMRNASets();
        ParseSequence();
    }

    private List<(int3, int)> CreateLisOfUniqueMRNASets()
    {
        List<(int3, int)> uniqueSets = new List<(int3, int)>();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    uniqueSets.Add((new int3(i, j, k), 0));
                }
            }
        }

        return uniqueSets;
    }

    private void ParseSequence()
    {
        MRNASequence mRNASequence = new MRNASequence();
        mRNASequence.ParseSequence();

        int leadEndIndex = mRNASequence.LeadBuffer;
        int trailStartIndex = mRNASequence.SequenceSets.Count - 1 - mRNASequence.TrailBuffer;

        for (int i = 0; i < mRNASequence.SequenceSets.Count - mRNA_SO.ActiveSetsCount; i++)
        {
            int3 currentSequence = mRNASequence.SequenceSets[i];
            int currentCount = 1;
            for (int j = 0; j < mRNA_SO.ActiveSetsCount; j++)
            {
                if (math.all(currentSequence == mRNASequence.SequenceSets[i + 1 + j]))
                {
                    currentCount += 1;
                }

                UpdateUniqueSets(currentSequence, currentCount);
            }
        }
    }

    private void UpdateUniqueSets(int3 set, int count)
    {
        for (int i = 0; i < uniqueSets.Count; i++)
        {
            if (math.all(uniqueSets[i].Item1 == set))
            {
                if (uniqueSets[i].Item2 < count)
                {
                    uniqueSets[i] = (uniqueSets[i].Item1, count);
                }
                break;
            }
        }
    }
}
