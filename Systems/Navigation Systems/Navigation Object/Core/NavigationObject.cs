using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;

public class NavigationObject : MonoBehaviour
{
    [SerializeField] private bool endOnSpline = false;
    [SerializeField] private Collider2D navMeshCollider;

    [SerializeField] private SplineContainer corePath;
    [SerializeField] private SplineContainer upperPath;
    [SerializeField] private SplineContainer lowerPath;

    [SerializeField, HideInInspector] private Transform corePathTransform;
    [SerializeField, HideInInspector] private Transform upperPathTransform;
    [SerializeField, HideInInspector] private Transform lowerPathTransform;
    private Vector3 localLocation;

    [SerializeField] private bool useWeightedRotation = false;

    [SerializeField] private int characterSortingOrder;
    [SerializeField] private NavigationObjectCameraData cameraData;

    [SerializeField] private List<RotationOverrideArea2D> rotationOverrideAreas;
    [SerializeField] private List<SortingOverrideArea2D> sortingOverrideAreas;

    private const float MAX_SEARCH_DISTANCE = 3f;
    //Note: This value may end up needing to be set per object
    private const float CLOSEST_NAVMESH_POINT_SEARCH_MODIFIER = .99f;

    public delegate void NavigationEvent(Vector3 navDestination, bool ignorePathingLimits);
    public static event NavigationEvent OnNavigate;

    public delegate void SortingOrderChangeEvent(NavigationObject navigationObject);
    public event SortingOrderChangeEvent OnSortingOrderChange;

    public virtual bool ForceFacingDirection => false;
    public virtual FacingDirection FacingDirection => FacingDirection.uninitialized;

    public int CharacterSortingOrder =>
        sortingOverrideAreas.Count == 0 ? characterSortingOrder : sortingOverrideAreas[sortingOverrideAreas.Count - 1].GetSortingOrder();

    public float ZPosition => navMeshCollider.transform.position.z;
    public NavigationObjectCameraData NavigationObjectCameraData => cameraData;

    public Collider2D NavMeshCollider => navMeshCollider;

    public void SetSplinePaths(SplineContainer newCorePath = null, SplineContainer newUpperPath = null, SplineContainer newLowerPath = null)
    {
        corePath = newCorePath == null ? corePath : newCorePath;
        upperPath = newUpperPath == null ? upperPath : newUpperPath;
        lowerPath = newLowerPath == null ? lowerPath : newLowerPath;
    }

    public void SetCharacterSortingOrder(int newSortingOrder)
    {
        characterSortingOrder = newSortingOrder;
    }

    public void SetCameraData(NavigationObjectCameraData newCameraData)
    {
        cameraData = newCameraData;
    }

    public void RegisterRotationOverrideArea(RotationOverrideArea2D rotationOverrideArea2D)
    {
        rotationOverrideAreas.Add(rotationOverrideArea2D);
    }

    public void DeregisterRotationOverrideArea(RotationOverrideArea2D rotationOverrideArea2D)
    {
        rotationOverrideAreas.Remove(rotationOverrideArea2D);
    }

    public void RegisterSortingOverrideArea(SortingOverrideArea2D sortingOverrideArea)
    {
        sortingOverrideAreas.Add(sortingOverrideArea);
        OnSortingOrderChange?.Invoke(this);
    }

    public void DeregisterSortingOverrideArea(SortingOverrideArea2D sortingOverrideArea)
    {
        sortingOverrideAreas.Remove(sortingOverrideArea);
        OnSortingOrderChange?.Invoke(this);
    }

    public void Navigate(Vector3 worldPosition, bool ignorePathingLimits = false)
    {
        Vector3 destination = FindDestination(worldPosition);
        OnNavigate?.Invoke(destination, ignorePathingLimits);
    }

    public void NavigatePrecalculatedDestination(Vector3 foundDestination)
    {
        OnNavigate?.Invoke(foundDestination, false);
    }

    public Vector3 FindDestination(Vector3 worldPosition)
    {
        var localPosition = corePathTransform.InverseTransformPoint(worldPosition);

        if (endOnSpline)
        {
            Vector3 closestSplinePoint = SplineUtilities.ClosestSplinePoint(corePath, localPosition, out _);
            closestSplinePoint = corePath.transform.TransformPoint(closestSplinePoint);

            return ClosestNavMeshPoint(corePath, localPosition, closestSplinePoint);
        }

        return ClosestNavMeshPoint(corePath, localPosition, worldPosition);
    }

    public virtual Quaternion GetRotationBasedOnLocation(Vector3 worldLocation, FacingDirection facingDirection)
    {
        float directionScalar = facingDirection == FacingDirection.left ? 1 : -1;

        if (rotationOverrideAreas.Count > 0)
        {
            foreach (RotationOverrideArea2D area in rotationOverrideAreas)
            {
                if (area.IsInOverrideArea(worldLocation))
                {
                    Vector3 rotationFromOverride = area.GetRotationValue(facingDirection == FacingDirection.left);
                    return Quaternion.Euler(rotationFromOverride);
                }
            }
        }

        float t_CorePath;
        localLocation = corePathTransform.InverseTransformPoint(worldLocation);
        Vector3 nearestPointCorePath = SplineUtilities.ClosestSplinePoint(corePath, localLocation, out t_CorePath);

        if (useWeightedRotation == false)
        {
            var rotationFromSpline = GetRotationFromSpline(corePath.Spline, t_CorePath, directionScalar);
            return rotationFromSpline;
        }

        float t_UpperPath;
        localLocation = upperPathTransform.InverseTransformPoint(worldLocation);
        Vector3 nearestPointUpperPath = SplineUtilities.ClosestSplinePoint(upperPath, localLocation, out t_UpperPath);

        float t_LowerPath;
        localLocation = lowerPathTransform.InverseTransformPoint(worldLocation);
        Vector3 nearestPointLowerPath = SplineUtilities.ClosestSplinePoint(lowerPath, localLocation, out t_LowerPath);

        var distance_core = Vector3.Distance(corePath.transform.TransformPoint(nearestPointCorePath), worldLocation);
        var distance_upper = Vector3.Distance(upperPath.transform.TransformPoint(nearestPointUpperPath), worldLocation);
        var distance_lower = Vector3.Distance(lowerPath.transform.TransformPoint(nearestPointLowerPath), worldLocation);

        if (distance_upper > distance_lower)
        {
            var core = GetRotationFromSpline(corePath.Spline, t_CorePath, directionScalar);
            var upper = GetRotationFromSpline(upperPath.Spline, t_UpperPath, directionScalar);

            return QuaternionUtilities.GetWeightedRotationFromAtoB(core, upper, distance_core, distance_upper);
        }
        else
        {
            var core = GetRotationFromSpline(corePath.Spline, t_CorePath, directionScalar);
            var lower = GetRotationFromSpline(lowerPath.Spline, t_LowerPath, directionScalar);

            return QuaternionUtilities.GetWeightedRotationFromAtoB(core, lower, distance_core, distance_upper);
        }
    }

    protected Quaternion GetRotationFromSpline(Spline spline, float t, float directionScalar)
    {
        var rightDirection = SplineUtility.EvaluateTangent(spline, t); // Gets our Left/Right?
        var forwardDirection = SplineUtility.EvaluateUpVector(spline, t); //Gets our Forward/Backward?
        //var upDirection = Vector3.Cross(rightDirection, forwardDirection); //Gets our up/down?

        Quaternion lookRotation = Quaternion.identity;

        switch (directionScalar)
        {
            case < 0:
                lookRotation = Quaternion.LookRotation(-forwardDirection, -rightDirection);
                break;
            case > 0:
                lookRotation = Quaternion.LookRotation(forwardDirection, rightDirection);
                break;
        }

        return lookRotation;
    }

    private Vector3 ClosestNavMeshPoint(SplineContainer splineContainer, Vector3 localSplinePosition, Vector3 worldPosition)
    {
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(navMeshCollider.ClosestPoint(worldPosition), out navMeshHit, MAX_SEARCH_DISTANCE, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }

        Vector3 closestSplinePoint = SplineUtilities.ClosestSplinePoint(splineContainer, localSplinePosition, out _);
        closestSplinePoint = splineContainer.transform.TransformPoint(closestSplinePoint);

        Vector3 intermediatePoint = closestSplinePoint +
            ((worldPosition - closestSplinePoint).normalized * MAX_SEARCH_DISTANCE * CLOSEST_NAVMESH_POINT_SEARCH_MODIFIER);

        if (NavMesh.SamplePosition(navMeshCollider.ClosestPoint(intermediatePoint), out navMeshHit, MAX_SEARCH_DISTANCE, NavMesh.AllAreas))
        {
            DebugWrapper.Log("No hit was detected on nav mesh at World Position, returning hit on nav mesh at Intermediate Point.", gameObject);
            return navMeshHit.position;
        }

        DebugWrapper.Log("No hit was detected on nav mesh, returning closest spline point.", gameObject);
        return closestSplinePoint;
    }
    private void OnValidate()
    {
        corePathTransform = corePath != null ? corePath.transform : null;
        upperPathTransform = upperPath != null ? upperPath.transform : null;
        lowerPathTransform = lowerPath != null ? lowerPath.transform : null;
    }

    protected virtual void Awake()
    {

    }
}