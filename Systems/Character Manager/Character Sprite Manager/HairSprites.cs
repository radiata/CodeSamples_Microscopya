using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HairSprites
{
    [SerializeField] public SpriteRenderer HairBack;
    [SerializeField] public SpriteRenderer HairFront_01;
    [SerializeField] public SpriteRenderer HairFront_02;
    [SerializeField] public SpriteRenderer HairMiddle;
    [SerializeField] public SpriteRenderer EyeBrow;

    public HairSprites(SpriteRenderer hairBack, SpriteRenderer hairFront_01, SpriteRenderer hairFront_02, SpriteRenderer hairMiddle, SpriteRenderer eyeBrow)
    {
        HairBack = hairBack;
        HairFront_01 = hairFront_01;
        HairFront_02 = hairFront_02;
        HairMiddle = hairMiddle;
        EyeBrow = eyeBrow;
    }

    [HideInInspector] public List<SpriteRenderer> spriteRenderers => new List<SpriteRenderer> { HairBack, HairFront_01, HairFront_02, HairMiddle, EyeBrow };
}