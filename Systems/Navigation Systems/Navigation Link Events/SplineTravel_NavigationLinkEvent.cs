using UnityEngine.Splines;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(SplineTravel_NavigationObject))]
public class SplineTravel_NavigationLinkEvent : NavigationLinkEvent
{
    [SerializeField] private CharacterAnimationSequences characterAnimationEvent;
    [SerializeField] private float transitionDuration;
    [SerializeField] private CharacterAnimationState endAnimationSequenceState;
    private Base_CharacterAnimationSequence characterAnimationSequence;

    [SerializeField] private SplineContainer animationPathSpline;
    [SerializeField] private SplineTravel_NavigationObject navigationObject;
    [SerializeField] private bool overrideNavigationObject;

    [SerializeField] private AnimationCurve positionCurve;

    [Header("Character Facing")]
    [SerializeField] private FacingDirection leadForcedDirection = FacingDirection.uninitialized;
    [SerializeField] private FacingDirection tailForcedDirection = FacingDirection.uninitialized;
    [SerializeField] private bool lockFacingDirection = false;

    [Header("Optimization")]
    [SerializeField] private bool isStatic = true;
    [SerializeField] private bool singleDirection = false;

    private float elapsedTime = 0;
    private bool invertTime = false;
    private Coroutine executionRoutine = null;

    private Vector3 navLinkStart;
    private Vector3 navLinkEnd;

    public float InterpolatedTime =>
        invertTime == false ? Mathf.Clamp01(elapsedTime / transitionDuration) : 1 - Mathf.Clamp01(elapsedTime / transitionDuration);

    public SplineContainer AnimationPathSpline =>
        animationPathSpline;

    public Vector3 AnimationPathSplineStartWorldPosition =>
        animationPathSpline.transform.TransformPoint((Vector3)animationPathSpline.Spline[0].Position);

    public Vector3 AnimationPathSplineEndWorldPosition =>
        animationPathSpline.transform.TransformPoint((Vector3)animationPathSpline.Spline[animationPathSpline.Spline.Count - 1].Position);

    public override bool OverrideNavigationObject => overrideNavigationObject;

    public override void ExecuteEvent(NavMeshAgent navMeshAgent, NavigationObject fromNavigationObject)
    {
        this.navMeshAgent = navMeshAgent;
        navMeshAgent.velocity = Vector3.zero;

        Vector3 characterPosition = navMeshAgent.transform.position;

        if (isStatic == false)
        {
            SetNavlinkPositions();
        }

        if(singleDirection != true)
        {
            float distanceToLead = Vector3.Distance(characterPosition, navLinkStart);
            float distanceToTail = Vector3.Distance(characterPosition, navLinkEnd);
            invertTime = distanceToLead > distanceToTail;
        }


        if (lockFacingDirection == true)
        {
            CharacterFacingLock.CharacterFacingLockRequest(invertTime == false ? leadForcedDirection : tailForcedDirection);
        }

        navigationObject.InitializeTransition(invertTime == false ? 0 : 1);


        characterAnimationSequence.InitializeSequence();
        executionRoutine = StartCoroutine(ExecutionRoutine());
    }

    private void UpdateNavAgentPosition(float interpolatedTime)
    {
        interpolatedTime = positionCurve.Evaluate(interpolatedTime);

        navMeshAgent.transform.position = animationPathSpline.EvaluatePosition(interpolatedTime);
    }

    [ContextMenu("Align Start & End with Spline")]
    private void AlignNavPointsAndSpline()
    {
        navMeshLink.startPoint = transform.InverseTransformPoint(AnimationPathSplineStartWorldPosition);
        navMeshLink.endPoint = transform.InverseTransformPoint(AnimationPathSplineEndWorldPosition);
    }

    private IEnumerator ExecutionRoutine()
    {
        elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            UpdateNavAgentPosition(InterpolatedTime);
            yield return null;
        }

        executionRoutine = null;
        characterAnimationSequence.EndSequence();

        if (lockFacingDirection == true)
        {
            CharacterFacingLock.CharacterFacingUnlockRequest();
        }

        InvokeOnNavigationLinkEventCompleted();
    }

    protected override void Reset()
    {
        base.Reset();
        navigationObject = GetComponent<SplineTravel_NavigationObject>();
    }

    protected override void Awake()
    {
        base.Awake();
        characterAnimationSequence = CharacterAnimationSequenceFactory.CreateSequence(characterAnimationEvent, transitionDuration, endAnimationSequenceState);
        SetNavlinkPositions();
    }

    private void SetNavlinkPositions()
    {
        navLinkStart = navMeshLink.transform.TransformPoint(navMeshLink.startPoint);
        navLinkEnd = navMeshLink.transform.TransformPoint(navMeshLink.endPoint);
    }
}
