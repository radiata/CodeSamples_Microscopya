using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshLink))]
public abstract class NavigationLinkEvent : MonoBehaviour
{
    [SerializeField] protected NavMeshLink navMeshLink;

    public delegate void NavigationLinkEventCompleted();
    public event NavigationLinkEventCompleted OnNavigationLinkEventCompleted;

    protected NavMeshAgent navMeshAgent = null;

    public virtual bool OverrideNavigationObject => false;

    public abstract void ExecuteEvent(NavMeshAgent navMeshAgent, NavigationObject fromNavigationObject);

    internal void InvokeOnNavigationLinkEventCompleted()
    {
        OnNavigationLinkEventCompleted?.Invoke();
    }

    protected virtual void Awake()
    {

    }

    protected virtual void OnValidate()
    {

    }

    protected virtual void Reset()
    {
        navMeshLink = GetComponent<NavMeshLink>();
    }
}
