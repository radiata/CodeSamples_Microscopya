using UnityEngine;

public class EventSequence : MonoBehaviour
{
    [SerializeField] private EventSequenceTriggerType triggerType;

    [SerializeField] private Base_Event[] sequenceEvents;

    [SerializeField] private SceneID sceneID;

    public delegate void EventSequenceCompleteEvent();
    public event EventSequenceCompleteEvent OnEventSequenceComplete;

    public delegate void EventSequenceStartEvent();
    public event EventSequenceStartEvent OnEventSequenceStarted;

    private int eventIndex = -1;

    public bool isSequenceComplete => eventIndex >= sequenceEvents.Length;

    public void StartOnCallSequence()
    {
        if (triggerType == EventSequenceTriggerType.OnCall)
        {
            StartSequence();
        }
    }

    private void Awake()
    {
        if (triggerType == EventSequenceTriggerType.OnAwake)
        {
            StartSequence();
        }
    }

    private void OnEnable()
    {
        if (triggerType == EventSequenceTriggerType.OnLoadingScreenComplete)
        {
            LoadingScreen.OnLoadingScreenComplete += TriggerStartSequence;
        }
    }

    private void OnDisable()
    {
        LoadingScreen.OnLoadingScreenComplete -= TriggerStartSequence;
    }

    private void TriggerStartSequence(SceneID sceneLoadID)
    {
        if(sceneID != sceneLoadID)
        {
            return;
        }

        StartSequence();
    }

    [ContextMenu("Start Sequence")]
    private void StartSequence()
    {
        eventIndex = -1;
        OnEventSequenceStarted?.Invoke();
        StartNextEvent();
    }

    private void StartNextEvent()
    {
        if (eventIndex > -1 && eventIndex < sequenceEvents.Length)
        {
            sequenceEvents[eventIndex].OnNextEventTrigger -= StartNextEvent;
        }

        eventIndex += 1;

        if (eventIndex >= sequenceEvents.Length)
        {
            SequenceComplete();
            return;
        }

        sequenceEvents[eventIndex].OnNextEventTrigger += StartNextEvent;
        sequenceEvents[eventIndex].StartEvent();
    }

    private void SequenceComplete()
    {
        OnEventSequenceComplete?.Invoke();
    }

    internal void DEBUG_RunSequence()
    {
        StartSequence();
    }
}
