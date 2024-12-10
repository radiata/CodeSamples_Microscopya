public struct VolumeSettings
{
    public float MainVolume;
    public float MusicVolume;
    public float SoundEffectsVolume;
    public bool Muted;

    public VolumeSettings(float mainVolume, float musicVolume, float soundEffectsVolume, bool muted)
    {
        MainVolume = mainVolume;
        MusicVolume = musicVolume;
        SoundEffectsVolume = soundEffectsVolume;
        Muted = muted;
    }
}
