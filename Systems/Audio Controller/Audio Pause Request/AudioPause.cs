using System.Collections.Generic;
using UnityEngine;

public class AudioPause : MonoBehaviour
{
    private static List<BehaviourRequest> activeRequests = new List<BehaviourRequest>();

    public BehaviourRequest RequestPause()
    {
        BehaviourRequest request = new BehaviourRequest();
        activeRequests.Add(request);

        AudioController.Instance.PauseAudio();
        LocationSoundPause._instance.PauseLBClips();

        return request;
    }

    private void OnReleaseRequest(int requestID)
    {
        int index = activeRequests.FindIndex(request => request.RequestID == requestID);
        if (index == -1)
        {
            return;
        }

        activeRequests.RemoveAt(index);

        if (activeRequests.Count == 0)
        {
            AudioController.Instance.ResumeAudio();
            LocationSoundPause._instance.ResumeLBClips();
        }
    }

    private void Awake()
    {
        BehaviourRequest.OnReleaseRequest += OnReleaseRequest;
    }

    private void OnDestroy()
    {
        BehaviourRequest.OnReleaseRequest -= OnReleaseRequest;

    }
}
