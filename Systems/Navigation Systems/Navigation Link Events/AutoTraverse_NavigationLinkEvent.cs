using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AutoTraverse_NavigationLinkEvent : NavigationLinkEvent
{
    private Coroutine executionRoutine = null;

    private float timeRequiredToTraverse;
    private Vector3 startWorldPosition;
    private Vector3 endWorldPosition;

    private float timeElapsed;

    public override void ExecuteEvent(NavMeshAgent navMeshAgent, NavigationObject fromNavigationObject)
    {
        this.navMeshAgent = navMeshAgent;

        startWorldPosition = navMeshAgent.transform.position;
        endWorldPosition = navMeshAgent.currentOffMeshLinkData.endPos;
        timeRequiredToTraverse = Vector3.Distance(startWorldPosition, endWorldPosition) / navMeshAgent.speed;
        timeElapsed = 0f;

        executionRoutine = StartCoroutine(ExecutionRoutine());
        InvokeOnNavigationLinkEventCompleted();
    }

    private void UpdateNavAgentPosition()
    {
        timeElapsed = Mathf.Clamp(timeElapsed + Time.deltaTime, 0, timeRequiredToTraverse);
        navMeshAgent.transform.position = Vector3.Lerp(startWorldPosition, endWorldPosition, (timeElapsed / timeRequiredToTraverse));
    }

    private bool CheckExecutionComplete()
    {
        return timeElapsed / timeRequiredToTraverse == 1;
    }

    private IEnumerator ExecutionRoutine()
    {

        while (CheckExecutionComplete() == false)
        {
            UpdateNavAgentPosition();
            yield return null;
        }

        executionRoutine = null;
        InvokeOnNavigationLinkEventCompleted();
    }
}
