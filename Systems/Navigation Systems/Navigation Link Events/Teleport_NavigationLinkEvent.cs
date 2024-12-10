using UnityEngine;
using UnityEngine.AI;

public class Teleport_NavigationLinkEvent : NavigationLinkEvent
{
    public override void ExecuteEvent(NavMeshAgent navMeshAgent, NavigationObject _)
    {
        navMeshAgent.transform.position = navMeshAgent.currentOffMeshLinkData.endPos;
        navMeshAgent.velocity = Vector3.zero;
        InvokeOnNavigationLinkEventCompleted();
    }
}
