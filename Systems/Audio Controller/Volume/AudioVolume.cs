using UnityEngine;

public class AudioVolume : MonoBehaviour
{
    public static AudioVolume Instance;
    [SerializeField] private Volume_Settings_SO volumeSettings;

    public delegate void MuteEvent(bool isMuted);
    public static event MuteEvent OnMuteChanged;

    public float MasterVolume => volumeSettings.MasterVolume;
    public float MusicVolume => volumeSettings.MusicVolume;
    public float SoundEffectsVolume => volumeSettings.SoundEffectsVolume;
    public bool IsMuted => volumeSettings.IsMuted;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadVolumeSettings();
    }

    public void Mute()
    {
        volumeSettings.SetMute(true);
        OnMuteChanged?.Invoke(true);
    }

    public void Unmute()
    {
        volumeSettings.SetMute(false);
        OnMuteChanged?.Invoke(false);
    }

    public void ToggleMute()
    {
        volumeSettings.SetMute(!volumeSettings.IsMuted);
    }

    private void LoadVolumeSettings()
    {
        volumeSettings.LoadVolumeSettings();
    }

    public void SetVolumeSettings(float masterVolume, float musicVolume, float soundEffectsVolume)
    {
        volumeSettings.SetVolume(masterVolume, musicVolume, soundEffectsVolume);
    }
}
