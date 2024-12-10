using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    private static List<BehaviourRequest> activeRequests = new List<BehaviourRequest>();

    public BehaviourRequest RequestTimeScale0()
    {
        BehaviourRequest request = new BehaviourRequest();
        activeRequests.Add(request);

        Time.timeScale = 0;

        return request;
    }

    private void OnReleaseRequest(int requestID)
    {
        int index = activeRequests.FindIndex(request => request.RequestID == requestID);

        if (index != -1)
        {
            activeRequests.RemoveAt(index);
        }

        if (activeRequests.Count == 0)
        {
            Time.timeScale = 1;
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
