using UnityEngine;

public class Event_SimpleActivateObject : Base_Event
{
    [SerializeField] private GameObject objectToActivate;
    internal override void HandleEvent()
    {
        objectToActivate.SetActive(true);

        CompleteEvent();
    }
}
