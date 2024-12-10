using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Volume Settings Data", menuName = "Custom Menus/Volume Settings/Volume Settings Scriptable Objects/Volume Settings")]
public class Volume_Settings_SO : ScriptableObject
{
    [Header("Adjustable Sound Mixers")]
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private float masterVolume;
    [SerializeField] private float defaultMasterVolume;

    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private float musicVolume;
    [SerializeField] private float defaultMusicVolume;

    [SerializeField] private AudioMixer soundEffectsMixer;
    [SerializeField] private float soundEffectsVolume;
    [SerializeField] private float defaultSoundEffectsVolume;


    [Header("Non-adjustable Mixers")]
    [SerializeField] private AudioMixer motorStepsMixer;
    [SerializeField] private float motorStepsVolume;

    [SerializeField] private AudioMixer mitoBoostMixer;
    [SerializeField] private float mitoBoostVolume;

    private bool isMuted = false;
    private float mutedVolume = -80;


    public float MasterVolume => masterVolume;
    public float MusicVolume => musicVolume;
    public float SoundEffectsVolume => soundEffectsVolume;
    public bool IsMuted => isMuted;

    public void ResetVolumeSettings()
    {
        SetVolume(defaultMasterVolume, defaultMusicVolume, defaultSoundEffectsVolume, false);
    }

    public void SetVolume(float masterVolume, float musicVolume, float soundEffectsVolume, bool isMuted)
    {
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.soundEffectsVolume = soundEffectsVolume;
        this.isMuted = isMuted;

        PlayerPrefs_Utilities.SetVolumeSettings(new VolumeSettings(masterVolume, musicVolume, soundEffectsVolume, isMuted));
        UpdateVolume();
    }

    public void SetVolume(float masterVolume, float musicVolume, float soundEffectsVolume)
    {
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.soundEffectsVolume = soundEffectsVolume;

        PlayerPrefs_Utilities.SetVolumeSettings(new VolumeSettings(masterVolume, musicVolume, soundEffectsVolume, isMuted));
        UpdateVolume();
    }

    public void SetMute(bool isMuted)
    {
        this.isMuted = isMuted;

        PlayerPrefs_Utilities.SetVolumeSettings(new VolumeSettings(masterVolume, musicVolume, soundEffectsVolume, isMuted));
        UpdateVolume();
    }

    internal void UpdateVolume()
    {
        if (isMuted == true)
        {
            masterMixer.SetFloat("Volume", mutedVolume);
            musicMixer.SetFloat("Volume", mutedVolume);
            soundEffectsMixer.SetFloat("Volume", mutedVolume);
            motorStepsMixer.SetFloat("Volume", mutedVolume);
            mitoBoostMixer.SetFloat("Volume", mutedVolume);
        }
        else
        {
            masterMixer.SetFloat("Volume", PercentVolumeToDB(masterVolume));
            musicMixer.SetFloat("Volume", PercentVolumeToDB(musicVolume));
            soundEffectsMixer.SetFloat("Volume", PercentVolumeToDB(soundEffectsVolume));
            motorStepsMixer.SetFloat("Volume", PercentVolumeToDB(motorStepsVolume));
            mitoBoostMixer.SetFloat("Volume", PercentVolumeToDB(mitoBoostVolume));
        }
    }

    public void LoadVolumeSettings()
    {
        VolumeSettings volumeSettings = PlayerPrefs_Utilities.GetVolumeSettings();

        masterVolume =
            volumeSettings.MainVolume < 0 ? defaultMasterVolume : volumeSettings.MainVolume;
        musicVolume =
            volumeSettings.MusicVolume < 0 ? defaultMusicVolume : volumeSettings.MusicVolume;
        soundEffectsVolume =
            volumeSettings.SoundEffectsVolume < 0 ? defaultSoundEffectsVolume : volumeSettings.SoundEffectsVolume;
        isMuted = volumeSettings.Muted;

        UpdateVolume();
    }

    private float PercentVolumeToDB(float percentVolume)
    {
        percentVolume = Mathf.Clamp01(percentVolume);

        return
            percentVolume == 0f ? mutedVolume : Mathf.Log10(percentVolume) * 20;
    }
}
