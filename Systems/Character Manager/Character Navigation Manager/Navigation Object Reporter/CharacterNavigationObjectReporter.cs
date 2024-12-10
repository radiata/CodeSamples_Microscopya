using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigationObjectReporter : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;

    public delegate void NavigationObjectChangedEvent(NavigationObject navigationObject);
    public static event NavigationObjectChangedEvent OnNavigationObjectChanged;

    public delegate void SortingOrderChangeEvent(NavigationObject navigationObject);
    public static event SortingOrderChangeEvent OnSortingOrderChange;

    public delegate void EnteredNavMeshLinkEvent(NavigationObject fromNavigationObject);
    public static event EnteredNavMeshLinkEvent OnEnteredNavMeshLink;

    private Object currentNavMeshOwner;
    private Object cachedNavMeshOwner;

    private static NavigationObject cachedNavigationObject;
    public static NavigationObject CachedNavigationObject => cachedNavigationObject;

    private NavigationObject overrideNavigationObject = null;
    private bool navigationObjectOverride = false;

    private NavigationLinkEvent navigationLinkEvent = null;

    public void SetOverrideNavigationObject(NavigationObject navigationObject)
    {
        overrideNavigationObject = navigationObject;
        UpdateNavigationObject(overrideNavigationObject);

        navigationObjectOverride = true;
    }

    public void ReleaseOverrideNavigationObject()
    {
        overrideNavigationObject = null;
        cachedNavMeshOwner = null;

        navigationObjectOverride = false;

        if(navigationLinkEvent != null)
        {
            navigationLinkEvent.OnNavigationLinkEventCompleted -= ReleaseOverrideNavigationObject;
            navigationLinkEvent = null;
        }
    }

    private void OnEnable()
    {
        cachedNavMeshOwner = null;
    }

    private void OnDisable()
    {
        if (navigationLinkEvent != null)
        {
            navigationLinkEvent.OnNavigationLinkEventCompleted -= ReleaseOverrideNavigationObject;
            navigationLinkEvent = null;
        }
    }

    private void Update()
    {
        if (navigationObjectOverride)
        {
            return;
        }

        currentNavMeshOwner = navMeshAgent.navMeshOwner;
        if (cachedNavMeshOwner != currentNavMeshOwner)
        {
            cachedNavMeshOwner = currentNavMeshOwner;
            UpdateNavigationObject();
        }
    }

    private void UpdateNavigationObject()
    {
        if (cachedNavigationObject != null)
        {
            cachedNavigationObject.OnSortingOrderChange -= UpdateSortingOrder;
        }

        bool enteredNavMeshLink = false;
        NavigationObject previousNavigationObject = cachedNavigationObject;

        switch (cachedNavMeshOwner)
        {
            case NavMeshSurface navMeshSurface:
                cachedNavigationObject = navMeshSurface.GetComponentInChildren<NavigationObject>();
                break;
            case NavMeshLink navMeshLink:
                cachedNavigationObject = navMeshLink.GetComponent<NavigationObject>();
                enteredNavMeshLink = true;
                break;
            default:
                cachedNavigationObject = null;
                break;
        }

        OnNavigationObjectChanged?.Invoke(cachedNavigationObject);

        if (enteredNavMeshLink == true)
        {
            NavigationLinkEvent navigationLinkEvent = cachedNavigationObject.GetComponent<NavigationLinkEvent>();
            if(navigationLinkEvent != null 
                && navigationLinkEvent.OverrideNavigationObject == true)
            {
                overrideNavigationObject = cachedNavigationObject;
                navigationObjectOverride = true;
                navigationLinkEvent.OnNavigationLinkEventCompleted -= ReleaseOverrideNavigationObject;
                navigationLinkEvent.OnNavigationLinkEventCompleted += ReleaseOverrideNavigationObject;
            }

            OnEnteredNavMeshLink?.Invoke(previousNavigationObject);
        }

        if (cachedNavigationObject != null)
        {
            cachedNavigationObject.OnSortingOrderChange += UpdateSortingOrder;
        }
    }

    private void UpdateNavigationObject(NavigationObject navigationObject)
    {
        if (cachedNavigationObject != null)
        {
            cachedNavigationObject.OnSortingOrderChange -= UpdateSortingOrder;
        }

        cachedNavigationObject = navigationObject;
        OnNavigationObjectChanged?.Invoke(cachedNavigationObject);

        if (cachedNavigationObject != null)
        {
            cachedNavigationObject.OnSortingOrderChange += UpdateSortingOrder;
        }
    }

    private void UpdateSortingOrder(NavigationObject navigationObject)
    {
        OnSortingOrderChange?.Invoke(navigationObject);
    }
}
