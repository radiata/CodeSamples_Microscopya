using UnityEngine;

public class Event_CharacterResume : Base_Event
{
    [SerializeField] private CharacterNavigationManager characterNavigationManager;
    internal override void HandleEvent()
    {
        characterNavigationManager.ResumeNavigation();
        CompleteEvent();
    }
}
