using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectsVolumeSlider;

    public void ChangeVolume()
    {
        float masterVolume = masterVolumeSlider.value;

        float musicVolume =
            musicVolumeSlider == null ? AudioVolume.Instance.MusicVolume : musicVolumeSlider.value;

        float soundEffectsVolume =
            soundEffectsVolumeSlider == null ? AudioVolume.Instance.SoundEffectsVolume : soundEffectsVolumeSlider.value;


        AudioVolume.Instance.SetVolumeSettings(masterVolume, musicVolume, soundEffectsVolume);
    }

    private void OnEnable()
    {
        masterVolumeSlider.SetValueWithoutNotify(AudioVolume.Instance.MasterVolume);

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(AudioVolume.Instance.MusicVolume);
        }
        if (soundEffectsVolumeSlider != null)
        {
            soundEffectsVolumeSlider.SetValueWithoutNotify(AudioVolume.Instance.SoundEffectsVolume);
        }
    }
}
