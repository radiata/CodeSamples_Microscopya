using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private TimeScale timeScale;
    private BehaviourRequest timeScaleChangeRequest;

    [SerializeField] private AudioPause audioPause;
    private BehaviourRequest pauseAudioRequest;

    [SerializeField] private PauseButton pauseButton;

    private void OnEnable()
    {
        ActivatePauseMenu();
    }

    private void OnDisable()
    {
        DeactivatePauseMenu();
    }

    internal void ActivatePauseMenu()
    {
        timeScaleChangeRequest = timeScale.RequestTimeScale0();
        pauseAudioRequest = audioPause.RequestPause();
    }

    internal void DeactivatePauseMenu()
    {
        timeScaleChangeRequest.ReleaseRequest();
        pauseAudioRequest.ReleaseRequest();

        timeScaleChangeRequest = null;
        pauseAudioRequest = null;
    }
}
