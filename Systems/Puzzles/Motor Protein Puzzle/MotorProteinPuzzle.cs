using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MotorProteinPuzzle : BasePuzzle
{
    [SerializeField] private MotorProteinAnimationController animationController;
    [SerializeField] private GameObject glowController;
    [SerializeField] private Transform characterAnchor;
    [SerializeField] private NavigationObject motorProteinNavigationObject;
    [SerializeField] private MotorProteinFootReceiver rightFootReceiver;
    [SerializeField] private MotorProteinFootReceiver leftFootReceiver;

    [SerializeField] private float delayJumpExecution = 1.25f;
    [SerializeField] private float delayUpdateComponents = 1f;
    [SerializeField] private float delayResumeMotor = .25f;

    private float jumpAnimationLength = 1f;

    [SerializeField] private CharacterNavigationObjectReporter characterNavigationObjectReporter;
    [SerializeField] private CharacterAnimationManager characterAnimationManager;
    [SerializeField] private CharacterRotationController characterRotationController;
    [SerializeField] private CharacterFacingReporter characterFacingReporter;
    [SerializeField] private CharacterNavigationManager characterNavigationManager;
    [SerializeField] private NavMeshAgent navMeshAgent;

    [SerializeField] private GameObject characterModel;

    [Header("Legacy Motor Jump Setup")]
    [SerializeField] private bool useLegacyMotorJump = true;

    [Header("Legacy End Sequence")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private TextTrigger hintTextTrigger;
    [SerializeField] private GameObject ATPCloud;
    [SerializeField] private GameObject virtualCameraFollowTarget;
    [SerializeField] private GameObject followTargetAnimator;

    [Header("Updated End Sequence")]
    [SerializeField] private EventSequence onPuzzleCompleteSequence;

    private MotorProteinFootReceiver footStepInitiator;

    public override void ActivatePuzzle()
    {
        puzzleManager.LockInPuzzleMode(true);

        glowController.SetActive(false);

        PauseMotor();

        InputHandler.Instance.ChangeInputMode(InputModes.Character_LockedNavigation);

        if (useLegacyMotorJump)
        {
            StartCoroutine(LegacyActivatePuzzle());
        }
        else
        {
            StandardActivatePuzzle();
        }
    }

    public override void DeactivatePuzzle()
    { }

    public override void InitializePuzzle_Awake()
    { }

    public override void InitializePuzzle_Start()
    { }

    public void PerformStep(MotorProteinFootReceiver footStepInitiator)
    {
        rightFootReceiver.DisableReceiver();
        leftFootReceiver.DisableReceiver();

        this.footStepInitiator = footStepInitiator;

        animationController.OnFootStepComplete -= CompleteStep;
        animationController.OnFootStepComplete += CompleteStep;

        animationController.PlayFootStep();
    }

    private void Awake()
    {
        jumpAnimationLength = characterAnimationManager.MotorPuzzleJumpAnimationClip.length;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        animationController.OnFootStepComplete -= CompleteStep;
    }

    private void StandardActivatePuzzle()
    {
        CharacterParentingController.ChangeParent(characterAnchor);

        float invokeDelay = delayJumpExecution;
        Invoke(nameof(CharacterJump), invokeDelay);

        invokeDelay += jumpAnimationLength + delayUpdateComponents;
        Invoke(nameof(UpdateCharacterComponents), invokeDelay);

        invokeDelay += delayResumeMotor;
        Invoke(nameof(ResumeMotor), invokeDelay);

        Invoke(nameof(EnableFirstReceiver), invokeDelay);
    }

    private IEnumerator LegacyActivatePuzzle()
    {
        characterFacingReporter.LockFacingDirection(FacingDirection.right);

        yield return null;
        UpdateCharacterComponents();
        characterRotationController.SnapRotation();

        yield return null;
        float invokeDelay = delayJumpExecution;
        Invoke(nameof(LegacyCharacterJump), invokeDelay);

        invokeDelay += characterAnimationManager.GetLegacyJumpAnimationLength;

        invokeDelay += delayResumeMotor;
        Invoke(nameof(LegacyResumeMotor), invokeDelay);

        Invoke(nameof(EnableFirstReceiver), invokeDelay);
    }

    private void LegacyCharacterJump()
    {
        CharacterParentingController.ChangeParent(characterAnchor);
        characterAnimationManager.HandleLegacyCharacterMotorPuzzleJump();
    }

    private void LegacyResumeMotor()
    {
        characterAnimationManager.ResyncRootToAnimator();
        characterAnimationManager.DisableRootMotion();

        animationController.ResumeIdle();
    }

    private void CharacterJump()
    {
        motorProteinNavigationObject.Navigate(characterAnchor.position);
    }

    private void UpdateCharacterComponents()
    {
        characterNavigationManager.enabled = false;
        characterNavigationObjectReporter.SetOverrideNavigationObject(motorProteinNavigationObject);
        characterRotationController.enabled = false;
        navMeshAgent.enabled = false;
    }

    private void ResumeMotor()
    {
        animationController.ResumeIdle();
    }

    private void PauseMotor()
    {
        animationController.PauseIdle();
    }

    private void EnableFirstReceiver()
    {
        rightFootReceiver.EnableReceiver();
    }

    private void CompleteStep(bool allStepsComplete)
    {
        animationController.OnFootStepComplete -= CompleteStep;

        footStepInitiator.ConvertATP();

        if (allStepsComplete == true)
        {
            CompletePuzzle();
            return;
        }

        if (rightFootReceiver == footStepInitiator)
        {
            leftFootReceiver.EnableReceiver();
        }
        else if (leftFootReceiver == footStepInitiator)
        {
            rightFootReceiver.EnableReceiver();
        }
    }

    private void CompletePuzzle()
    {
        //release atp cloud
        ATPCloud.transform.SetParent(null);
        //hide hint object
        hintTextTrigger.FadeOut();

        rightFootReceiver.ReleaseADP();
        leftFootReceiver.ReleaseADP();

        InvokeOnPuzzleCompleted();
        onPuzzleCompleteSequence.StartOnCallSequence();

        followTargetAnimator.SetActive(true);
        virtualCamera.Follow = virtualCameraFollowTarget.transform;
    }
}
