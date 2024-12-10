using UnityEngine;

public class BehaviourRequestHandler : MonoBehaviour
{
    [SerializeField] private TimeScale timeScale;
    private BehaviourRequest timeScaleChangeRequest;

    [SerializeField] private AudioPause audioPause;
    private BehaviourRequest pauseAudioRequest;

    private void OnEnable()
    {
        Activate();
    }

    private void OnDisable()
    {
        Deactivate();
    }

    internal void Activate()
    {
        timeScaleChangeRequest = timeScale.RequestTimeScale0();
        pauseAudioRequest = audioPause.RequestPause();
    }

    internal void Deactivate()
    {
        timeScaleChangeRequest.ReleaseRequest();
        pauseAudioRequest.ReleaseRequest();

        timeScaleChangeRequest = null;
        pauseAudioRequest = null;
    }
}
