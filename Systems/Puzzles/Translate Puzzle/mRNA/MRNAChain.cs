using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRNAChain : MonoBehaviour
{
    [SerializeField] private MRNA_SO mRNA_SO;
    [SerializeField] private MRNAPool mRNAPool;
    private string mRNAHolderName(int i) => $"mRNA_Holder_{i:00}";
    [SerializeField] private List<GameObject> mRNAHolders = new List<GameObject>();
    private int leadMRNAHolder = 0;
    private List<MRNAPoolObject> mRNAPoolObjects = new List<MRNAPoolObject>();

    [SerializeField] private Transform rootTransform;
    [SerializeField] private Transform chainHolder;

    [SerializeField] private int activeIndex = 3;
    private int resetIndex;

    [SerializeField] Material spriteGreyScale;
    [SerializeField] Material spriteFullColor;

    private MRNASequence mRNASequence = new MRNASequence();

    private Coroutine shiftChainCoroutine;

    public delegate void MRNAChainShiftCompleteEvent();
    public event MRNAChainShiftCompleteEvent OnMRNAChainShiftCompleted;

    [SerializeField] private float shiftAnimationTime = 2f;
    [SerializeField] private float chainShiftStartAnimationTime = 1f;
    private float bufferTime = .1f;

    [SerializeField] private List<Animator> chainShiftAnimators;
    [SerializeField] private string chainShiftTrigger = "ChainShiftTrigger";

    public int EndIndex => mRNASequence.SequenceSets.Count - 1 - mRNASequence.TrailBuffer;
    public int ActiveIndex => activeIndex;

    private bool canEnd = false;

    public MRNASet GetActiveMRNASet()
    {
        int targetIndex = (leadMRNAHolder + (mRNAHolders.Count / 2)) % mRNAHolders.Count;
        return mRNAPoolObjects[targetIndex].mRNASet;
    }
    public Transform GetActiveIndexTRNAAnchor()
    {
        int targetIndex = (leadMRNAHolder + (mRNAHolders.Count / 2)) % mRNAHolders.Count;
        return mRNAPoolObjects[targetIndex].mRNASet.Anchor_tRNA;
    }

    public void Initialize()
    {
        mRNASequence.ParseSequence();
        ResetMRNAChain();
    }

    [ContextMenu("Create mRNA Holders")]
    private void CreateMRNAHolders()
    {
        int centerValue = mRNA_SO.ActiveSetsCount / 2;

        for (int i = 0; i < mRNA_SO.ActiveSetsCount; i++)
        {
            GameObject mRNAHolder = new GameObject(mRNAHolderName(i));
            mRNAHolder.transform.SetParent(chainHolder);
            mRNAHolder.transform.localRotation = Quaternion.identity;
            mRNAHolder.transform.localScale = Vector3.one;

            mRNAHolder.transform.localPosition = Vector3.right * (i - centerValue) * mRNA_SO.mRNAWidth;

            mRNAHolders.Add(mRNAHolder);
        }
    }

    public void ResetMRNAChain()
    {
        leadMRNAHolder = 0;

        for (int i = 0; i < mRNAPoolObjects.Count; i++)
        {
            if (mRNAPoolObjects[i] != null)
            {
                mRNAPool.Recycle(mRNAPoolObjects[i]);
            }
        }

        mRNAPoolObjects.Clear();

        int centerValue = mRNAHolders.Count / 2;
        for (int i = 0; i < mRNAHolders.Count; i++)
        {
            mRNAHolders[i].transform.localPosition = Vector3.right * (i - centerValue) * mRNA_SO.mRNAWidth;
        }

        PlaceStartingMRNA();
    }
    private void PlaceStartingMRNA()
    {
        int centerValue = mRNAHolders.Count / 2;
        for (int i = 0; i < mRNAHolders.Count; i++)
        {
            int mRNASequenceIndex = activeIndex + (i - centerValue);
            if (mRNASequenceIndex < 0)
            {
                mRNAPoolObjects.Add(mRNAPool.GetEmpty());
            }
            else
            {
                mRNAPoolObjects.Add(mRNAPool.GetNext(mRNASequence.SequenceSets[mRNASequenceIndex]));
            }

            if (mRNASequenceIndex < mRNASequence.LeadBuffer)
            {
                mRNAPoolObjects[i].mRNASet.ApplyMaterial(spriteGreyScale);
            }
            else
            {
                mRNAPoolObjects[i].mRNASet.ApplyMaterial(spriteFullColor);
            }

            mRNAPoolObjects[i].mRNASet.transform.SetParent(mRNAHolders[i].transform);
            mRNAPoolObjects[i].mRNASet.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }

    public void ShiftChainBy(int shiftBy)
    {
        shiftChainCoroutine = StartCoroutine(ShiftChain_Animator(shiftBy));
    }

    public void SetChainToNextIndex(bool invokeEvent = false)
    {
        UpdatePoolObjectsTargetPositions();
        UpdateMRNAHoldersPosition(1f);
        UpdateMRNAHolders();
        activeIndex += 1;

        if (invokeEvent == true)
        {
            OnMRNAChainShiftCompleted?.Invoke();
        }
    }

    public void ResetActiveIndex()
    {
        activeIndex = resetIndex;
    }

    public void SetChainToEnd()
    {
        activeIndex = mRNASequence.SequenceSets.Count - ((mRNAHolders.Count / 2) + 1) - mRNASequence.TrailBuffer - 1;
        canEnd = true;
    }

    public void ResetPosition()
    {
        transform.localPosition = Vector3.zero;
    }

    private void Awake()
    {
        resetIndex = activeIndex;
    }

    private IEnumerator ShiftChain_Animator(int shiftBy)
    {
        float elapsedTime = 0;
        UpdatePoolObjectsTargetPositions();

        foreach (Animator animator in chainShiftAnimators)
        {
            animator.SetTrigger(chainShiftTrigger);
        }

        float normalTime;
        while (elapsedTime < shiftAnimationTime + bufferTime)
        {
            if (elapsedTime >= chainShiftStartAnimationTime)
            {
                normalTime = Mathf.Clamp01((elapsedTime - chainShiftStartAnimationTime) / (shiftAnimationTime - chainShiftStartAnimationTime));
                UpdateMRNAHoldersPosition(normalTime);
            }

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        UpdateMRNAHolders();
        activeIndex += 1;
        OnMRNAChainShiftCompleted?.Invoke();
    }

    private void UpdatePoolObjectsTargetPositions()
    {
        Vector3 startPosition;
        Vector3 nextPosition;

        for (int i = 0; i < mRNAHolders.Count; i++)
        {
            startPosition = mRNAHolders[i].transform.localPosition;
            nextPosition = mRNAHolders[i].transform.localPosition + (Vector3.left * mRNA_SO.mRNAWidth);
            mRNAPoolObjects[i]?.mRNASet.SetLocalPositions(startPosition, nextPosition);
        }
    }

    private void UpdateMRNAHoldersPosition(float normalTime)
    {
        (Vector3, Vector3) targets;
        for (int i = 0; i < mRNAHolders.Count; i++)
        {
            targets = mRNAPoolObjects[i].mRNASet.GetLocalPositions;
            mRNAHolders[i].transform.localPosition = Vector3.Lerp(targets.Item1, targets.Item2, normalTime);
        }
    }

    private void UpdateMRNAHolders()
    {
        mRNAHolders[leadMRNAHolder].transform.localPosition = Vector3.right * ((mRNAHolders.Count - 1) / 2) * mRNA_SO.mRNAWidth;
        int indexToSpawn = activeIndex + (mRNAHolders.Count / 2) + 1;

        if (canEnd == false
            && indexToSpawn >= mRNASequence.SequenceSets.Count - mRNASequence.TrailBuffer)
        {
            ResetActiveIndex();
            indexToSpawn = activeIndex + (mRNAHolders.Count / 2) + 1;
        }

        if (mRNAPoolObjects[leadMRNAHolder] != null)
        {
            mRNAPool.Recycle(mRNAPoolObjects[leadMRNAHolder]);
        }

        if (indexToSpawn >= mRNASequence.SequenceSets.Count)
        {
            mRNAPoolObjects[leadMRNAHolder] = mRNAPool.GetEmpty();
        }
        else
        {
            mRNAPoolObjects[leadMRNAHolder] = mRNAPool.GetNext(mRNASequence.SequenceSets[indexToSpawn]);
        }

        if (indexToSpawn >= mRNASequence.SequenceSets.Count - mRNASequence.TrailBuffer)
        {
            mRNAPoolObjects[leadMRNAHolder].mRNASet.ApplyMaterial(spriteGreyScale);
        }
        else
        {
            mRNAPoolObjects[leadMRNAHolder].mRNASet.ApplyMaterial(spriteFullColor);
        }

        mRNAPoolObjects[leadMRNAHolder].mRNASet.transform.SetParent(mRNAHolders[leadMRNAHolder].transform);
        mRNAPoolObjects[leadMRNAHolder].mRNASet.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        leadMRNAHolder = (leadMRNAHolder + 1) % mRNAHolders.Count;
    }
}
