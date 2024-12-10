using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public struct MusicTrackData
{
    public MusicTrack MusicTrack_ID;
    public AssetReferenceT<AudioClip> AssetReference;
}

