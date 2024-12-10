using UnityEngine;

public class Event_ObjectiveComplete : Base_Event
{
    [SerializeField] private ObjectiveData objectiveData;

    internal override void HandleEvent()
    {
        objectiveData.SetComplete();
        CompleteEvent();
    }
}
