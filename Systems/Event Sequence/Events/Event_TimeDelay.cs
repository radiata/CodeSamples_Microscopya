using System.Collections;
using UnityEngine;

public class Event_TimeDelay : Base_Event
{
    [Tooltip("If set to unscaled time, then setting Time.TimeScale to 0 will not impact the sequence")]
    [SerializeField] private bool unscaledTime = false;
    [SerializeField] private float delayLength;

    private Coroutine timeDelayRoutine;

    internal override void HandleEvent()
    {
        if(timeDelayRoutine != null)
        {
            StopCoroutine(timeDelayRoutine);
        }

        timeDelayRoutine = StartCoroutine(TimeDelay());
    }

    IEnumerator TimeDelay()
    {
        float elapsedTime = 0;

        while (elapsedTime < delayLength)
        {
            yield return null;
            elapsedTime += unscaledTime == true ? Time.unscaledDeltaTime : Time.deltaTime;
        }

        timeDelayRoutine = null;
        CompleteEvent();
    }
}
