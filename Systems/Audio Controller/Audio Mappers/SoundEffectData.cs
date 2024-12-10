using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public struct SoundEffectData
{
    public SoundEffect SoundEffect_ID;
    public AssetReferenceT<AudioClip> AssetReference;
}
