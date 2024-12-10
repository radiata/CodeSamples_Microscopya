using UnityEngine;

public class MRNALogSequence : MonoBehaviour
{
    [SerializeField] private Vector2Int indexRange = new Vector2Int(3, 16);


    [ContextMenu("Debug Log Sequence - Range")]
    private void DebugLoqSequence()
    {
        MRNASequence mRNASequence = new MRNASequence();
        mRNASequence.DebugLogSequence(indexRange.x, indexRange.y);
    }

    [ContextMenu("Debug Log Sequence Pairs - Range")]
    private void DebugLoqSequencePairs()
    {
        MRNASequence mRNASequence = new MRNASequence();
        mRNASequence.DebugLogSequencePairs(indexRange.x, indexRange.y);
    }

    [ContextMenu("Debug Log Sequence - ALL")]
    private void DebugLoqSequenceAll()
    {
        MRNASequence mRNASequence = new MRNASequence();
        mRNASequence.DebugLogSequence();
    }
}
