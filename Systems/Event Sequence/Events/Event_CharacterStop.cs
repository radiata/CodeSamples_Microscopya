using UnityEngine;

public class Event_CharacterStop : Base_Event
{
    [SerializeField] private CharacterNavigationManager characterNavigationManager;
    internal override void HandleEvent()
    {
        characterNavigationManager.StopNavigation();
        CompleteEvent();
    }
}
