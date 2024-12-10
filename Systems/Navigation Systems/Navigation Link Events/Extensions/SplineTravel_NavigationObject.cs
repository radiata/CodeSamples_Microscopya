using UnityEngine;

public class SplineTravel_NavigationObject : NavigationObject
{
    [Header("Spline Travel Variables")]
    //source rotation
    [SerializeField] private GameObject leadNavigation_RootObject;
    private NavigationObject leadNavigationObject;
    [SerializeField] private FacingDirection leadFacingDirection;
    private Quaternion leadOriginRotation;
    private Quaternion leadDestinationRotation;

    //end rotation
    [SerializeField] private GameObject tailNavigation_RootObject;
    private NavigationObject tailNavigationObject;
    [SerializeField] private FacingDirection tailFacingDirection;
    private Quaternion tailOriginRotation;
    private Quaternion tailDestinationRotation;

    [SerializeField] private bool useSplineRotation = false;
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private SplineTravel_NavigationLinkEvent navigationLinkEvent;

    private Quaternion activeOriginRotation;
    private Quaternion activeDestinationRotation;
    private FacingDirection activeFacingDirection;

    public override bool ForceFacingDirection => true;
    public override FacingDirection FacingDirection => activeFacingDirection;

    public void InitializeTransition(int startingTime)
    {
        if (startingTime == 0)
        {
            activeOriginRotation = leadOriginRotation;
            activeDestinationRotation = leadDestinationRotation;
            activeFacingDirection = leadFacingDirection;
        }
        else if (startingTime == 1)
        {
            // active origin and destination are flipped because of the inverted time scale when going from spline tail to spline lead
            activeOriginRotation = tailDestinationRotation;
            activeDestinationRotation = tailOriginRotation;
            activeFacingDirection = tailFacingDirection;
        }
    }

    public override Quaternion GetRotationBasedOnLocation(Vector3 location, FacingDirection facingDirection)
    {
        if (useSplineRotation == true)
        {
            float directionScalar = facingDirection == FacingDirection.left ? 1 : -1;
            var rotationFromSpline = GetRotationFromSpline(navigationLinkEvent.AnimationPathSpline.Spline, navigationLinkEvent.InterpolatedTime, directionScalar);
            return rotationFromSpline;
        }
        else
        {
            //the first frame is not correct for rotation, naybe we need a hard reset on nav object change?
            return Quaternion.Slerp(activeOriginRotation, activeDestinationRotation, rotationCurve.Evaluate(navigationLinkEvent.InterpolatedTime));
        }
    }

    public bool isLeadNavigationObject(NavigationObject comparable)
    {
        if (leadNavigationObject == comparable)
        {
            return true;
        }

        return false;
    }

    protected override void Awake()
    {
        leadNavigationObject = leadNavigation_RootObject.GetComponentInChildren<NavigationObject>();
        tailNavigationObject = tailNavigation_RootObject.GetComponentInChildren<NavigationObject>();

        leadOriginRotation = leadNavigationObject.GetRotationBasedOnLocation(navigationLinkEvent.AnimationPathSplineStartWorldPosition, leadFacingDirection);
        leadDestinationRotation = tailNavigationObject.GetRotationBasedOnLocation(navigationLinkEvent.AnimationPathSplineEndWorldPosition, leadFacingDirection);

        tailOriginRotation = tailNavigationObject.GetRotationBasedOnLocation(navigationLinkEvent.AnimationPathSplineEndWorldPosition, tailFacingDirection);
        tailDestinationRotation = leadNavigationObject.GetRotationBasedOnLocation(navigationLinkEvent.AnimationPathSplineStartWorldPosition, tailFacingDirection);
    }

    protected void Reset()
    {
        navigationLinkEvent = GetComponent<SplineTravel_NavigationLinkEvent>();
    }
}
