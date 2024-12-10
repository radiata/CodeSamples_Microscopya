using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TRNA Data", menuName = "Puzzles/Translation Puzzle/TRNA Data")]
public class TRNAData_SO : ScriptableObject
{
    [SerializeField] private GameObject prefabTRNA;

    [SerializeField] private Sprite sprite_EndCodon;
    [SerializeField] private Sprite sprite_Generic;
    [SerializeField] private Sprite sprite_AAA;
    [SerializeField] private Sprite sprite_AAC;
    [SerializeField] private Sprite sprite_AAG;
    [SerializeField] private Sprite sprite_AAU;
    [SerializeField] private Sprite sprite_ACA;
    [SerializeField] private Sprite sprite_ACC;
    [SerializeField] private Sprite sprite_ACG;
    [SerializeField] private Sprite sprite_ACU;
    [SerializeField] private Sprite sprite_AGA;
    [SerializeField] private Sprite sprite_AGC;
    [SerializeField] private Sprite sprite_AGG;
    [SerializeField] private Sprite sprite_AGU;
    [SerializeField] private Sprite sprite_AUA;
    [SerializeField] private Sprite sprite_AUC;
    [SerializeField] private Sprite sprite_AUG;
    [SerializeField] private Sprite sprite_AUU;
    [SerializeField] private Sprite sprite_CAA;
    [SerializeField] private Sprite sprite_CAC;
    [SerializeField] private Sprite sprite_CAG;
    [SerializeField] private Sprite sprite_CAU;
    [SerializeField] private Sprite sprite_CCA;
    [SerializeField] private Sprite sprite_CCC;
    [SerializeField] private Sprite sprite_CCG;
    [SerializeField] private Sprite sprite_CCU;
    [SerializeField] private Sprite sprite_CGA;
    [SerializeField] private Sprite sprite_CGC;
    [SerializeField] private Sprite sprite_CGG;
    [SerializeField] private Sprite sprite_CGU;
    [SerializeField] private Sprite sprite_CUA;
    [SerializeField] private Sprite sprite_CUC;
    [SerializeField] private Sprite sprite_CUG;
    [SerializeField] private Sprite sprite_CUU;
    [SerializeField] private Sprite sprite_GAA;
    [SerializeField] private Sprite sprite_GAC;
    [SerializeField] private Sprite sprite_GAG;
    [SerializeField] private Sprite sprite_GAU;
    [SerializeField] private Sprite sprite_GCA;
    [SerializeField] private Sprite sprite_GCC;
    [SerializeField] private Sprite sprite_GCG;
    [SerializeField] private Sprite sprite_GCU;
    [SerializeField] private Sprite sprite_GGA;
    [SerializeField] private Sprite sprite_GGC;
    [SerializeField] private Sprite sprite_GGG;
    [SerializeField] private Sprite sprite_GGU;
    [SerializeField] private Sprite sprite_GUA;
    [SerializeField] private Sprite sprite_GUC;
    [SerializeField] private Sprite sprite_GUG;
    [SerializeField] private Sprite sprite_GUU;
    [SerializeField] private Sprite sprite_UAA;
    [SerializeField] private Sprite sprite_UAC;
    [SerializeField] private Sprite sprite_UAG;
    [SerializeField] private Sprite sprite_UAU;
    [SerializeField] private Sprite sprite_UCA;
    [SerializeField] private Sprite sprite_UCC;
    [SerializeField] private Sprite sprite_UCG;
    [SerializeField] private Sprite sprite_UCU;
    [SerializeField] private Sprite sprite_UGA;
    [SerializeField] private Sprite sprite_UGC;
    [SerializeField] private Sprite sprite_UGG;
    [SerializeField] private Sprite sprite_UGU;
    [SerializeField] private Sprite sprite_UUA;
    [SerializeField] private Sprite sprite_UUC;
    [SerializeField] private Sprite sprite_UUG;
    [SerializeField] private Sprite sprite_UUU;

    public TRNA CreateTRNA(TRNAType tRNAType, AminoAcidChain aminoAcidChain, List<GameObject> validReceivers, PuzzleManager puzzleManager, Transform parent)
    {
        GameObject prefab = Instantiate(prefabTRNA);
        TRNA tRNA = prefab.GetComponentInChildren<TRNA>();
        tRNA.SetTRNAType(tRNAType);
        tRNA.SetSprite(GetSpriteByType(tRNAType));
        tRNA.SetAminoAcidChain(aminoAcidChain);
        tRNA.SetReceiver(validReceivers);
        tRNA.SetPuzzleManager(puzzleManager);
        tRNA.SetParent(parent);

        return tRNA;
    }

    public Sprite GetSpriteByType(TRNAType tRNAType)
    {
        switch (tRNAType)
        {
            case TRNAType.None:
                return sprite_Generic;
            case TRNAType.AAA:
                return sprite_AAA;
            case TRNAType.AAC:
                return sprite_AAC;
            case TRNAType.AAG:
                return sprite_AAG;
            case TRNAType.AAU:
                return sprite_AAU;
            case TRNAType.ACA:
                return sprite_ACA;
            case TRNAType.ACC:
                return sprite_ACC;
            case TRNAType.ACG:
                return sprite_ACG;
            case TRNAType.ACU:
                return sprite_ACU;
            case TRNAType.AGA:
                return sprite_AGA;
            case TRNAType.AGC:
                return sprite_AGC;
            case TRNAType.AGG:
                return sprite_AGG;
            case TRNAType.AGU:
                return sprite_AGU;
            case TRNAType.AUA:
                return sprite_AUA;
            case TRNAType.AUC:
                return sprite_AUC;
            case TRNAType.AUG:
                return sprite_AUG;
            case TRNAType.AUU:
                return sprite_AUU;
            case TRNAType.CAA:
                return sprite_CAA;
            case TRNAType.CAC:
                return sprite_CAC;
            case TRNAType.CAG:
                return sprite_CAG;
            case TRNAType.CAU:
                return sprite_CAU;
            case TRNAType.CCA:
                return sprite_CCA;
            case TRNAType.CCC:
                return sprite_CCC;
            case TRNAType.CCG:
                return sprite_CCG;
            case TRNAType.CCU:
                return sprite_CCU;
            case TRNAType.CGA:
                return sprite_CGA;
            case TRNAType.CGC:
                return sprite_CGC;
            case TRNAType.CGG:
                return sprite_CGG;
            case TRNAType.CGU:
                return sprite_CGU;
            case TRNAType.CUA:
                return sprite_CUA;
            case TRNAType.CUC:
                return sprite_CUC;
            case TRNAType.CUG:
                return sprite_CUG;
            case TRNAType.CUU:
                return sprite_CUU;
            case TRNAType.GAA:
                return sprite_GAA;
            case TRNAType.GAC:
                return sprite_GAC;
            case TRNAType.GAG:
                return sprite_GAG;
            case TRNAType.GAU:
                return sprite_GAU;
            case TRNAType.GCA:
                return sprite_GCA;
            case TRNAType.GCC:
                return sprite_GCC;
            case TRNAType.GCG:
                return sprite_GCG;
            case TRNAType.GCU:
                return sprite_GCU;
            case TRNAType.GGA:
                return sprite_GGA;
            case TRNAType.GGC:
                return sprite_GGC;
            case TRNAType.GGG:
                return sprite_GGG;
            case TRNAType.GGU:
                return sprite_GGU;
            case TRNAType.GUA:
                return sprite_GUA;
            case TRNAType.GUC:
                return sprite_GUC;
            case TRNAType.GUG:
                return sprite_GUG;
            case TRNAType.GUU:
                return sprite_GUU;
            case TRNAType.UAA:
                return sprite_UAA;
            case TRNAType.UAC:
                return sprite_UAC;
            case TRNAType.UAG:
                return sprite_UAG;
            case TRNAType.UAU:
                return sprite_UAU;
            case TRNAType.UCA:
                return sprite_UCA;
            case TRNAType.UCC:
                return sprite_UCC;
            case TRNAType.UCG:
                return sprite_UCG;
            case TRNAType.UCU:
                return sprite_UCU;
            case TRNAType.UGA:
                return sprite_UGA;
            case TRNAType.UGC:
                return sprite_UGC;
            case TRNAType.UGG:
                return sprite_UGG;
            case TRNAType.UGU:
                return sprite_UGU;
            case TRNAType.UUA:
                return sprite_UUA;
            case TRNAType.UUC:
                return sprite_UUC;
            case TRNAType.UUG:
                return sprite_UUG;
            case TRNAType.UUU:
                return sprite_UUU;
            case TRNAType.EndCodon:
                return sprite_EndCodon;
            default:
                return sprite_Generic;
        }

    }
}

