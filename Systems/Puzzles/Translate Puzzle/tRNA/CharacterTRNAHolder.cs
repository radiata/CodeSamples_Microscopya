using UnityEngine;

public class CharacterTRNAHolder : MonoBehaviour
{
    [SerializeField] private Transform rootTransform;
    [SerializeField] private Transform characterParent;

    [SerializeField] private Vector3 characterOffset;
    [SerializeField] private Vector3 characterScale = Vector3.one;

    [SerializeField] private TRNACharacterSlot[] tRNASlots;

    [SerializeField] private TranslatePuzzle translatePuzzle;

    public bool AttemptToCollect(CollectableTRNA tRNA)
    {
        TRNACharacterSlot slot = null;

        for (int i = 0; i < tRNASlots.Length; i++)
        {
            if (tRNASlots[i].isOccupied == false)
            {
                slot = tRNASlots[i];
                break;
            }
        }

        if (slot == null)
        {
            return false;
        }

        slot.CaptureTRNA(tRNA);
        translatePuzzle.CollectedPuzzlePiece();
        return true;
    }

    [ContextMenu("Attach to Character")]
    public void AttachToCharacter()
    {
        rootTransform.SetParent(characterParent);
        rootTransform.localScale = characterScale;
        rootTransform.localPosition = characterOffset;
        rootTransform.localRotation = Quaternion.identity;
    }

    public void PuzzleCompleted()
    {
        Destroy(rootTransform.gameObject);
    }

    public void ReleaseToPuzzle()
    {
        foreach (var slot in tRNASlots)
        {
            if(slot.isOccupied == true)
            {
                slot.ReleaseTRNAToPuzzle();
            }
        }
    }
}
