using System.Collections;
using UnityEngine;

public abstract class Base_Event : MonoBehaviour
{
    [SerializeField] private NextEventTriggerType nextEventTriggerType;
    [SerializeField] private float delayTime = 0f;

    public delegate void NextEventTriggerEvent();
    public event NextEventTriggerEvent OnNextEventTrigger;

    private Coroutine timeDelayRoutine;

    public void StartEvent()
    {
        if(nextEventTriggerType == NextEventTriggerType.Immediate)
        {
            Raise_OnNextEventTrigger();
        }

        if (nextEventTriggerType == NextEventTriggerType.TimeDelay)
        {
            timeDelayRoutine = StartCoroutine(NextEventTimeDelay(delayTime));
        }

        HandleEvent();
    }

    internal abstract void HandleEvent();
    
    internal void CompleteEvent()
    {
        if(nextEventTriggerType == NextEventTriggerType.OnComplete)
        {
            Raise_OnNextEventTrigger();
        }
    }

    private void Raise_OnNextEventTrigger()
    {
        OnNextEventTrigger?.Invoke();
    }

    private IEnumerator NextEventTimeDelay(float timeDelay)
    {
        float timeElapsed = 0;

        while(timeElapsed < timeDelay)
        {
            yield return null;
            timeElapsed += Time.deltaTime;
        }

        Raise_OnNextEventTrigger();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
