using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimationManager : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float velocityAnimationWeight = .15f;

    [SerializeField] private CharacterAnimationState characterAnimationState;
    [SerializeField] private Animator characterAnimator;

    [SerializeField] private AnimationClip jumpAnimationClip;

    [SerializeField] private AnimationClip motorPuzzleJumpAnimationClip;
    [SerializeField] private AnimationClip legacyMotorPuzzleJumpAnimationClip;
    public AnimationClip MotorPuzzleJumpAnimationClip => motorPuzzleJumpAnimationClip;

    public void SetAnimationState(CharacterAnimationState newCharacterAnimationState, float animationSpeed = 1f)
    {
        characterAnimator.speed = animationSpeed;

        switch (newCharacterAnimationState)
        {
            case CharacterAnimationState.Idle:
                SetIdleState();
                break;
            case CharacterAnimationState.Running:
                SetRunningState();
                break;
            case CharacterAnimationState.Transition:
                SetTransitionState();
                break;
            case CharacterAnimationState.JumpState:
                SetJumpState();
                break;
            case CharacterAnimationState.SlideState:
                SetSlideState();
                break;
            case CharacterAnimationState.MotorPuzzleJumpState:
                SetMotorPuzzleJumpState();
                break;
            case CharacterAnimationState.RunTransition:
                SetRunTransitionState();
                break;
        }
    }

    private void FixedUpdate()
    {
        HandleState();
    }

    private void Awake()
    {
        SetAnimationState(CharacterAnimationState.Running);
    }

    private void HandleState()
    {
        switch (characterAnimationState)
        {
            case CharacterAnimationState.Idle:
                HandleIdleState();
                break;
            case CharacterAnimationState.Running:
                HandleRunningState();
                break;
            case CharacterAnimationState.Transition:
                HandleTransitionState();
                break;
            case CharacterAnimationState.JumpState:
                HandleJumpState();
                break;
            case CharacterAnimationState.SlideState:
                HandleSlideState();
                break;
            case CharacterAnimationState.MotorPuzzleJumpState:
                HandleMotorPuzzleJumpState();
                break;
            case CharacterAnimationState.RunTransition:
                HandleRunTransitionState();
                break;
        }
    }

    private void HandleRunningState()
    {
        var newSpeed = navMeshAgent.velocity.magnitude * velocityAnimationWeight;
        characterAnimator.SetFloat("runAmount", newSpeed);

        HandleSliding();
    }

    private void HandleSliding()
    {
        if (navMeshAgent.velocity.sqrMagnitude == 0)
        {
            characterAnimator.SetBool("sliding", false);
        }
    }

    private void HandleIdleState()
    { }

    private void HandleJumpState()
    { }

    private void HandleSlideState()
    { }

    private void HandleMotorPuzzleJumpState()
    { }

    private void HandleTransitionState()
    {
        throw new System.NotImplementedException();
    }

    private void HandleRunTransitionState()
    { }

    private void SetRunningState()
    {
        characterAnimator.SetBool("running", true);
        characterAnimator.SetBool("sliding", false);
        characterAnimationState = CharacterAnimationState.Running;
    }

    private void SetIdleState()
    {
        characterAnimator.SetBool("running", false);
        characterAnimator.SetTrigger("Idle");
        characterAnimationState = CharacterAnimationState.Idle;
    }

    private void SetJumpState()
    {
        characterAnimator.SetBool("running", false);
        characterAnimator.SetBool("sliding", false);
        characterAnimator.SetTrigger("Jump");
        characterAnimationState = CharacterAnimationState.JumpState;
    }

    private void SetSlideState()
    {
        characterAnimator.SetBool("running", false);
        characterAnimator.SetBool("sliding", false);
        characterAnimator.SetTrigger("Slide");

        characterAnimationState = CharacterAnimationState.SlideState;
    }

    private void SetMotorPuzzleJumpState()
    {
        characterAnimator.SetBool("running", false);
        characterAnimator.SetBool("sliding", false);
        characterAnimator.SetTrigger("MotorPuzzleJump");
        characterAnimationState = CharacterAnimationState.MotorPuzzleJumpState;
    }

    private void SetTransitionState()
    {
        throw new System.NotImplementedException();
        characterAnimationState = CharacterAnimationState.Transition;
    }

    private void SetRunTransitionState()
    {
        characterAnimator.SetBool("running", true);
        characterAnimator.SetBool("sliding", false);

        var newSpeed = navMeshAgent.velocity.magnitude * velocityAnimationWeight;
        characterAnimator.SetFloat("runAmount", newSpeed);

        characterAnimationState = CharacterAnimationState.RunTransition;
    }

    private void OnEnable()
    {
        NavigationObject.OnNavigate += HandleNavigateEvent;
        CharacterNavigationVelocityController.OnCharacterBrake += HandleBrakeEvent;

        CharacterJumpSequence.OnCharacterJumpSequenceStart += HandleCharacterJumpSequenceStart;
        CharacterJumpSequence.OnCharacterJumpSequenceEnd += HandleCharacterJumpSequenceEnd;

        CharacterSlideSequence.OnCharacterSlideSequenceStart += HandleCharacterSlideSequenceStart;
        CharacterSlideSequence.OnCharacterSlideSequenceEnd += HandleCharacterSlideSequenceEnd;

        CharacterMotorPuzzleJumpSequence.OnCharacterMotorPuzzleJumpSequenceStart += HandleCharacterMotorPuzzleJumpSequenceStart;
        CharacterMotorPuzzleJumpSequence.OnCharacterMotorPuzzleJumpSequenceEnd += HandleCharacterMotorPuzzleJumpSequenceEnd;

        CharacterRunSequence.OnCharacterRunSequenceStart += HandleCharacterRunSequenceStart;
        CharacterRunSequence.OnCharacterRunSequenceEnd += HandleCharacterRunSequenceEnd;
    }

    private void OnDisable()
    {
        NavigationObject.OnNavigate -= HandleNavigateEvent;
        CharacterNavigationVelocityController.OnCharacterBrake -= HandleBrakeEvent;

        CharacterJumpSequence.OnCharacterJumpSequenceStart -= HandleCharacterJumpSequenceStart;
        CharacterJumpSequence.OnCharacterJumpSequenceEnd -= HandleCharacterJumpSequenceEnd;

        CharacterSlideSequence.OnCharacterSlideSequenceStart -= HandleCharacterSlideSequenceStart;
        CharacterSlideSequence.OnCharacterSlideSequenceEnd -= HandleCharacterSlideSequenceEnd;

        CharacterMotorPuzzleJumpSequence.OnCharacterMotorPuzzleJumpSequenceStart -= HandleCharacterMotorPuzzleJumpSequenceStart;
        CharacterMotorPuzzleJumpSequence.OnCharacterMotorPuzzleJumpSequenceEnd -= HandleCharacterMotorPuzzleJumpSequenceEnd;

        CharacterRunSequence.OnCharacterRunSequenceStart -= HandleCharacterRunSequenceStart;
        CharacterRunSequence.OnCharacterRunSequenceEnd -= HandleCharacterRunSequenceEnd;
    }

    private void HandleNavigateEvent(Vector3 navDestination, bool ignorePathingLimits)
    {
        if (characterAnimationState == CharacterAnimationState.JumpState
            || characterAnimationState == CharacterAnimationState.SlideState
            || characterAnimationState == CharacterAnimationState.MotorPuzzleJumpState)
        {
            return;
        }

        SetRunningState();
    }

    private void HandleBrakeEvent()
    {
        if (characterAnimator.GetBool("sliding") == false || characterAnimator.GetBool("running") == true)
        {
            characterAnimator.SetBool("running", false);
            characterAnimator.SetBool("sliding", true);
        }
    }

    private void HandleCharacterJumpSequenceStart(float duration)
    {
        var playbackSpeed = jumpAnimationClip.length / duration;
        SetAnimationState(CharacterAnimationState.JumpState, playbackSpeed);
    }
    private void HandleCharacterJumpSequenceEnd(CharacterAnimationState nextAnimationState)
    {
        SetAnimationState(nextAnimationState);
    }

    private void HandleCharacterSlideSequenceStart(float duration)
    {
        SetAnimationState(CharacterAnimationState.SlideState);
    }
    private void HandleCharacterSlideSequenceEnd(CharacterAnimationState nextAnimationState)
    {
        SetAnimationState(nextAnimationState);
    }

    private void HandleCharacterMotorPuzzleJumpSequenceStart(float duration)
    {
        var playbackSpeed = motorPuzzleJumpAnimationClip.length / duration;
        SetAnimationState(CharacterAnimationState.MotorPuzzleJumpState, playbackSpeed);
    }
    private void HandleCharacterMotorPuzzleJumpSequenceEnd(CharacterAnimationState nextAnimationState)
    {
        SetAnimationState(nextAnimationState);
    }

    private void HandleCharacterRunSequenceStart(float duration)
    {
        SetAnimationState(CharacterAnimationState.RunTransition);
    }

    private void HandleCharacterRunSequenceEnd(CharacterAnimationState nextAnimationState)
    {
        SetAnimationState(nextAnimationState);
    }

    public void HandleLegacyCharacterMotorPuzzleJump()
    {
        characterAnimator.SetBool("running", false);
        characterAnimator.SetBool("sliding", false);
        characterAnimator.SetTrigger("MJump");

        characterAnimationState = CharacterAnimationState.MotorPuzzleJumpState;
    }
    public float GetLegacyJumpAnimationLength => legacyMotorPuzzleJumpAnimationClip.length;

    public void ResyncRootToAnimator()
    {
        Transform root = characterAnimator.transform.parent;
        characterAnimator.transform.SetParent(null);
        root.transform.position = characterAnimator.transform.position;
        root.transform.rotation = characterAnimator.transform.rotation;
        characterAnimator.transform.SetParent(root);
    }

    public void DisableRootMotion()
    {
        characterAnimator.applyRootMotion = false;
    }
}
