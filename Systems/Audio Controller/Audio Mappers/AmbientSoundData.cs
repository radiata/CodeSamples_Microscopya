using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public struct AmbientSoundData
{
    public AmbientSound AmbientSound_ID;
    public AssetReferenceT<AudioClip> AssetReference;
}
