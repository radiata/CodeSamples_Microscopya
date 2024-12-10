using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource oneShotSoundSource;

    [SerializeField] private MusicTrackMapper_SO musicTrackLibrary;
    [SerializeField] private SoundEffectMapper_SO soundEffectLibrary;

    private Coroutine waitForMusicTrackToEnd;
    private MusicTrack queuedMusicTrack = MusicTrack.None;
    private bool loopQueuedMusicTrack = false;

    private AssetReference lastMusicRequest = null;

    private List<LoadedAudioAsset> loadedAudioAssets = new List<LoadedAudioAsset>();

    public MusicTrack QueuedMusicTrack => queuedMusicTrack;

    internal void InitializeAudioControllerSettings(AudioController_Settings_SO audioController_Settings_SO)
    {
        return;
        throw new NotImplementedException();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance.gameObject);
        }
        Instance = this;
    }

    private LoadedAudioAsset LoadAudioAsset(AssetReference soundAddress, AudioType audioType, int audioID, bool loop, bool autoPlay)
    {
        AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(soundAddress);

        LoadedAudioAsset loadedAudioAsset = new LoadedAudioAsset(handle, audioType, audioID);
        loadedAudioAssets.Add(loadedAudioAsset);

        if (autoPlay == true)
        {
            if (loop == true)
            {
                handle.Completed += OnAudioAssetLoaded_PlayLooped;
            }
            else
            {
                handle.Completed += OnAudioAssetLoaded_PlayUnlooped;
            }
        }

        return loadedAudioAsset;
    }

    private void OnAudioAssetLoaded_PlayUnlooped(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            LoadedAudioAsset loadedAudioAsset = loadedAudioAssets.First(search => handle.Equals(search.operationHandle));

            switch (loadedAudioAsset.audioType)
            {
                case AudioType.MusicTrack:
                    musicSource.clip = loadedAudioAsset.operationHandle.Result;
                    musicSource.loop = false;
                    musicSource.Play();
                    break;
                case AudioType.SoundEffect:
                    oneShotSoundSource.PlayOneShot(loadedAudioAsset.operationHandle.Result);
                    break;
            }
        }
    }

    private void OnAudioAssetLoaded_PlayLooped(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            LoadedAudioAsset loadedAudioAsset = loadedAudioAssets.First(search => handle.Equals(search.operationHandle));

            switch (loadedAudioAsset.audioType)
            {
                case AudioType.MusicTrack:
                    musicSource.clip = loadedAudioAsset.operationHandle.Result;
                    musicSource.loop = true;
                    musicSource.Play();
                    break;
                case AudioType.SoundEffect:
                    soundSource.clip = loadedAudioAsset.operationHandle.Result;
                    soundSource.loop = true;
                    soundSource.Play();
                    break;
            }
        }
    }

    public LoadedAudioAsset PreloadAudioAsset(MusicTrack musicTrack)
    {
        return PreloadAudioAsset(AudioType.MusicTrack, (int)musicTrack);
    }
    public LoadedAudioAsset PreloadAudioAsset(SoundEffect soundEffect)
    {
        return PreloadAudioAsset(AudioType.SoundEffect, (int)soundEffect);
    }

    private LoadedAudioAsset PreloadAudioAsset(AudioType audioType, int audioID)
    {
        LoadedAudioAsset preloadedAudio =
            loadedAudioAssets.FirstOrDefault(search => search.audioType == audioType && search.audioID == audioID);

        if (preloadedAudio != null)
        {
            preloadedAudio.referenceCount += 1;
            return preloadedAudio;
        }
        else
        {
            AssetReference assetReference = null;

            switch (audioType)
            {
                case AudioType.MusicTrack:
                    assetReference = musicTrackLibrary.GetMusicTrackAssetReference((MusicTrack)audioID);
                    break;
                case AudioType.SoundEffect:
                    assetReference = soundEffectLibrary.GetSoundEffectAssetReference((SoundEffect)audioID);
                    break;
            }

            return LoadAudioAsset(assetReference, AudioType.SoundEffect, audioID, false, false);
        }
    }

    public void UnloadAsset(LoadedAudioAsset loadedAudioAsset)
    {
        loadedAudioAsset.referenceCount -= 1;

        if(loadedAudioAsset.referenceCount <= 0)
        {
            Addressables.Release(loadedAudioAsset.operationHandle);
            loadedAudioAssets.Remove(loadedAudioAsset);
        }
    }

    public void PlayMusic(MusicTrack musicTrack, bool loopTrack)
    {
        if(musicTrack == MusicTrack.None)
        {
            return;
        }

        AssetReference soundAddress = musicTrackLibrary.GetMusicTrackAssetReference(musicTrack);

        if (soundAddress == lastMusicRequest)
        {
            musicSource.loop = loopTrack;
            return;
        }

        LoadedAudioAsset preloadedMusic = loadedAudioAssets.FirstOrDefault(search => search.audioType == AudioType.MusicTrack && search.audioID == (int)musicTrack);

        if (preloadedMusic != null)
        {
            musicSource.clip = preloadedMusic.operationHandle.Result;
            musicSource.loop = loopTrack;
            musicSource.Play();
        }
        else
        {
            DebugWrapper.LogWarning($"Music Track not found/preloaded. MT: {(int)musicTrack} - {musicTrack.ToString()}", gameObject);

            LoadAudioAsset(
                soundAddress
                , AudioType.MusicTrack
                , (int)musicTrack
                , loopTrack
                , true);
        }

        ClearQueuedMusicTrack();
        lastMusicRequest = soundAddress;
    }

    public void QueueMusicTrack(MusicTrack musicTrack, bool loopTrack, float delayBetweenSongs = 0)
    {
        if(musicTrack == MusicTrack.None)
        {
            return;
        }

        if (waitForMusicTrackToEnd != null)
        {
            DebugWrapper.LogError("A track is already queued", gameObject);
        }

        queuedMusicTrack = musicTrack;
        loopQueuedMusicTrack = loopTrack;

        if (waitForMusicTrackToEnd != null)
        {
            StopCoroutine(waitForMusicTrackToEnd);
        }

        waitForMusicTrackToEnd = StartCoroutine(WaitForClipToEnd(musicSource, PlayQueuedMusicTrack, delayBetweenSongs));
    }

    public void ClearQueuedMusicTrack()
    {
        queuedMusicTrack = MusicTrack.None;
        loopQueuedMusicTrack = false;

        if (waitForMusicTrackToEnd != null)
        {
            StopCoroutine(waitForMusicTrackToEnd);
            waitForMusicTrackToEnd = null;
        }
    }

    private void PlayQueuedMusicTrack()
    {
        PlayMusic(queuedMusicTrack, loopQueuedMusicTrack);
    }

    public void PlaySoundEffect(SoundEffect soundEffect, bool loopSound)
    {
        if(soundEffect == SoundEffect.None)
        {
            return;
        }

        LoadedAudioAsset preloadedSound = loadedAudioAssets.FirstOrDefault(search => search.audioType == AudioType.SoundEffect && search.audioID == (int)soundEffect);

        if (preloadedSound != null)
        {
            if (loopSound == false)
            {
                oneShotSoundSource.PlayOneShot(preloadedSound.operationHandle.Result);
            }
            else
            {
                soundSource.clip = preloadedSound.operationHandle.Result;
                soundSource.loop = loopSound;
                soundSource.Play();
            }
        }
        else
        {
            DebugWrapper.LogWarning($"Sound Effect not found/preloaded. SE: {(int)soundEffect} - {soundEffect.ToString()}", gameObject);

            LoadAudioAsset(
                soundEffectLibrary.GetSoundEffectAssetReference(soundEffect)
                , AudioType.SoundEffect
                , (int)soundEffect
                , loopSound
                , true);
        }
    }

    public void StopSoundEffect()
    {
        soundSource.Stop();
    }

    public void StopOneShotSoundEffect()
    {
        oneShotSoundSource.Stop();
    }

    public void PauseAudio()
    {
        musicSource.Pause();
        soundSource.Pause();
        //oneShotSoundSource.Pause();
    }

    public void ResumeAudio()
    {
        musicSource?.UnPause();
        soundSource?.UnPause();
        oneShotSoundSource?.UnPause();
    }

    private IEnumerator WaitForClipToEnd(AudioSource source, UnityAction callback, float additionalDelay)
    {
        yield return new WaitWhile(() => source.time < source.clip.length);

        while (additionalDelay > 0)
        {
            yield return null;
            additionalDelay -= Time.deltaTime;
        }

        callback?.Invoke();
        waitForMusicTrackToEnd = null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        foreach(LoadedAudioAsset loadedAudioAsset in loadedAudioAssets)
        {
            Addressables.Release(loadedAudioAsset.operationHandle);
        }
    }
}
