using System.Collections.Generic;
using UnityEngine;

public class TRNACloud : MonoBehaviour
{
    [SerializeField] private TRNAPuzzleSlot[] slots;

    [SerializeField] private TRNAData_SO tRNAData;

    [SerializeField] private List<TRNAType> tRNARequired = new List<TRNAType>()
    {
        TRNAType.CUC,
        TRNAType.CUC,
        TRNAType.GGC,
        TRNAType.GUC,
        TRNAType.AGU,
        TRNAType.CUA,
        TRNAType.GGA,
        TRNAType.UCG,
        TRNAType.CAG,
        TRNAType.CUC,
        TRNAType.GGG
    };

    [SerializeField] private TranslatePuzzle translatePuzzle;
    [SerializeField] private Transform tRNACloud_Root;

    public bool AllSlotsVacant()
    {
        foreach (var slot in slots)
        {
            if (slot.isOccupied == true)
            {
                return false;
            }
        }
        return true;
    }

    public void SpawnTRNA(TRNAType solutionType, int spawnCount)
    {
        if (DoesTRNASolutionAlreadyExist(solutionType) == false)
        {
            var vacantSlot = GetVacantSlot();
            var tRNA = CreateTRNA(solutionType);
            spawnCount -= 1;
            AssignTRNAToSlot(tRNA, vacantSlot);
            RemoveTRNAFromPool(tRNA.Type_tRNA);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            var vacantSlot = GetVacantSlot();
            var tRNA = CreateRandomTRNA(tRNARequired);
            AssignTRNAToSlot(tRNA, vacantSlot);
            RemoveTRNAFromPool(tRNA.Type_tRNA);
        }
    }

    private void AssignTRNAToSlot(TRNA tRNA, TRNAPuzzleSlot slot)
    {
        slot.ReceiveTRNA(tRNA);
    }

    public void AssignCharacterHeldTRNAToSlot(CollectableTRNA tRNA, TRNAPuzzleSlot slot)
    {
        slot.ReceiveCharacterHeldTRNA(tRNA);
    }

    private TRNAPuzzleSlot GetVacantSlot()
    {
        int randomSlot = Random.Range(0, slots.Length);

        for (int i = 0; i <= slots.Length; i++)
        {
            if (slots[randomSlot].isOccupied)
            {
                randomSlot = (randomSlot + 1) % slots.Length;
            }
            else
            {
                return slots[randomSlot];
            }
        }

        return null;
    }

    public TRNAPuzzleSlot ReserveVacantSlot()
    {
        int randomSlot = Random.Range(0, slots.Length);

        for (int i = 0; i <= slots.Length; i++)
        {
            if (slots[randomSlot].isOccupied)
            {
                randomSlot = (randomSlot + 1) % slots.Length;
            }
            else
            {
                slots[randomSlot].ReserveSlot();
                return slots[randomSlot];
            }
        }

        return null;
    }

    public bool DoesTRNASolutionAlreadyExist(TRNAType solutionType)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isOccupied == true)
            {
                if (slots[i].HeldTRNAType == solutionType)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private TRNA CreateTRNA(TRNAType typeToCreate)
    {
        return tRNAData.CreateTRNA(
            typeToCreate
            , translatePuzzle.AminoAcidChain
            , new List<GameObject>() { translatePuzzle.TranslationReceiver.gameObject }
            , translatePuzzle.PuzzleManager
            , tRNACloud_Root);
    }

    private TRNA CreateRandomTRNA(List<TRNAType> validTRNAList)
    {
        TRNAType typeToCreate = validTRNAList[Random.Range(0, validTRNAList.Count)];
        return CreateTRNA(typeToCreate);
    }

    private void RemoveTRNAFromPool(TRNAType typeToRemove)
    {
        tRNARequired.Remove(typeToRemove);
    }
}
