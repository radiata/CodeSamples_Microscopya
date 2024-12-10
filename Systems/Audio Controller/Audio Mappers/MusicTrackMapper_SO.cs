using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Music Track Library", menuName = "Audio Library Maps/Scriptable Objects/Music Track Library")]
public class MusicTrackMapper_SO : ScriptableObject
{
    [SerializeField] private List<MusicTrackData> musicTracks;

    public AssetReference GetMusicTrackAssetReference(MusicTrack musicTrack)
    {
        if(musicTrack == MusicTrack.None)
        {
            return null;
        }

        return musicTracks.First(search => search.MusicTrack_ID == musicTrack).AssetReference;
    }
}
