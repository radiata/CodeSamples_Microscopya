using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CharacterNavigationManager : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private CharacterNavigationVelocityController characterNavigationVelocityController;

    private CharacterPathFinder characterPathFinder = new CharacterPathFinder();

    private bool pathRestrictedByMaxDistance = false;
    private float maxPathingDistance;

    private bool pathRestrictedByCameraView = false;

    private NavigationLinkEvent activeNavigationLinkEvent = null;

    public void SetPathingRestrictions(bool distanceRestriction, float maxDistance, bool cameraViewRestriction)
    {
        pathRestrictedByMaxDistance = distanceRestriction;
        maxPathingDistance = maxDistance;
        pathRestrictedByCameraView = cameraViewRestriction;
    }

    public bool EvaluatePath(Vector3 navDestination, bool ignorePathingLimits, out NavMeshPath navMeshPath)
    {
        if (activeNavigationLinkEvent != null)
        {
            navMeshPath = null;
            return false;
        }

        Camera referenceCamera = null;
        float? maxPathingDistance = null;

        if (pathRestrictedByCameraView)
        {
            referenceCamera = CameraManager.Instance.ActiveCamera;
        }

        if (pathRestrictedByMaxDistance)
        {
            maxPathingDistance = this.maxPathingDistance;
        }

        return characterPathFinder.EvaluatePath(navMeshAgent, navDestination, ignorePathingLimits, out navMeshPath, referenceCamera, maxPathingDistance);
    }

    public void OnNavigate(NavMeshPath navMeshPath)
    {
        characterPathFinder.PathCompleted(navMeshPath);
    }

    private void OnNavigate(Vector3 navDestination, bool ignorePathingLimits)
    {
        if (activeNavigationLinkEvent != null)
        {
            return;
        }

        Camera referenceCamera = null;
        float? maxPathingDistance = null;

        if (pathRestrictedByCameraView)
        {
            referenceCamera = CameraManager.Instance.ActiveCamera;
        }

        if (pathRestrictedByMaxDistance)
        {
            maxPathingDistance = this.maxPathingDistance;
        }

        characterPathFinder.GetPath(navMeshAgent, navDestination, ignorePathingLimits, referenceCamera, maxPathingDistance);
    }

    private void SetDestination(NavMeshPath navMeshPath)
    {
        if (navMeshPath == null)
        {
            return;
        }

        navMeshAgent.SetPath(navMeshPath);
        characterNavigationVelocityController.SetDestination(navMeshPath);
    }

    private void OnNavigationLinkEventCompleted()
    {
        navMeshAgent.CompleteOffMeshLink();
        activeNavigationLinkEvent.OnNavigationLinkEventCompleted -= OnNavigationLinkEventCompleted;
        activeNavigationLinkEvent = null;
    }

    private void OnEnteredNavMeshLink(NavigationObject fromNavigationObject)
    {
        activeNavigationLinkEvent = ((NavMeshLink)navMeshAgent.navMeshOwner).gameObject.GetComponent<NavigationLinkEvent>();
        activeNavigationLinkEvent.OnNavigationLinkEventCompleted += OnNavigationLinkEventCompleted;
        activeNavigationLinkEvent.ExecuteEvent(navMeshAgent, fromNavigationObject);
    }

    private void OnResearchModeStateChange(bool isEnabled)
    {
        if (isEnabled == true)
        {
            StopNavigation();
        }
        else
        {
            ResumeNavigation();
        }
    }

    public void StopNavigation()
    {
        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = true;
    }

    public void ResumeNavigation()
    {
        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = false;
    }

    private void OnEnable()
    {
        activeNavigationLinkEvent = null;

        NavigationObject.OnNavigate += OnNavigate;
        CharacterPathFinder.OnGetPathCompleted += SetDestination;
        CharacterNavigationObjectReporter.OnEnteredNavMeshLink += OnEnteredNavMeshLink;
        ResearchModeState.OnResearchModeStateChanged += OnResearchModeStateChange;
    }

    private void OnDisable()
    {
        NavigationObject.OnNavigate -= OnNavigate;
        CharacterPathFinder.OnGetPathCompleted -= SetDestination;
        CharacterNavigationObjectReporter.OnEnteredNavMeshLink -= OnEnteredNavMeshLink;
        ResearchModeState.OnResearchModeStateChanged -= OnResearchModeStateChange;

        if (activeNavigationLinkEvent != null)
        {
            activeNavigationLinkEvent.OnNavigationLinkEventCompleted -= OnNavigationLinkEventCompleted;
        }
    }
}
