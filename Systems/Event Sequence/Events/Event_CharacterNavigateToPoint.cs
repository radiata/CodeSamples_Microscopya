using UnityEngine;
using UnityEngine.AI;

public class Event_CharacterNavigateToPoint : Base_Event
{
    [SerializeField] private CharacterNavigationManager characterNavigationManager;
    [SerializeField] private Transform destination;

    internal override void HandleEvent()
    {
        NavMeshPath navMeshPath;
        characterNavigationManager.EvaluatePath(destination.position, true, out navMeshPath);

        characterNavigationManager.OnNavigate(navMeshPath);

        CompleteEvent();
    }
}
