using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Sound Effect Library", menuName = "Audio Library Maps/Scriptable Objects/Sound Effect Library")]

public class SoundEffectMapper_SO : ScriptableObject
{
    [SerializeField] private List<SoundEffectData> soundEffects;

    public AssetReference GetSoundEffectAssetReference(SoundEffect soundEffect)
    {
        if(soundEffect == SoundEffect.None)
        {
            return null;
        }

        return soundEffects.First(search => search.SoundEffect_ID == soundEffect).AssetReference;
    }
}
