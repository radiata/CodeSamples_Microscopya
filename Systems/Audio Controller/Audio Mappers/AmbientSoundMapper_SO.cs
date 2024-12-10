using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Ambient Sound Library", menuName = "Audio Library Maps/Scriptable Objects/Ambient Sound Library")]
public class AmbientSoundMapper_SO : ScriptableObject
{
    [SerializeField] private List<AmbientSoundData> ambientSounds;

    public AssetReference GetMusicTrackAssetReference(AmbientSound ambientSound)
    {
        if(ambientSound == AmbientSound.None)
        {
            return null;
        }

        return ambientSounds.First(search => search.AmbientSound_ID == ambientSound).AssetReference;
    }

}
