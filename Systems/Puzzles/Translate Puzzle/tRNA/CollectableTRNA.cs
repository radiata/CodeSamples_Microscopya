using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CollectableTRNA : MonoBehaviour
{
    [SerializeField] private bool isCollectable = false;
    [SerializeField] private TRNA tRNA;

    [SerializeField] private Transform rootObject;
    [SerializeField] private FreeFloat freeFloat;
    [SerializeField] private BehaviourChangingPuzzlePiece_Handler behaviourChangingPuzzlePiece_Handler;
    [SerializeField] private CharacterRotationController characterRotationController;

    [SerializeField] private SpriteRenderer glowSpriteRenderer;
    [SerializeField] private SoundEffect onCollect_Sound = SoundEffect.TubulinDelivered;

    [SerializeField] private float timeToScale = 1.5f;
    [SerializeField] private Vector3 characterScale = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private Vector3 puzzleScale = Vector3.one;

    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float followEasingDistance = 2.5f;

    [Header("Rotation Tweening")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float updateTolerance = 40f;
    [SerializeField] private float zPuzzleOffset = 90f;
    private Tweener rotationTween;
    private float rotationTweenTargetZ;

    [Header("Objective Updater")]
    [SerializeField] private ObjectiveID_ER objectiveID;

    private Transform targetToFollow = null;

    private Coroutine lerpScaleRoutine = null;
    private Coroutine lerpRotationRoutine = null;
    private Coroutine followRoutine = null;

    private FacingDirection lastFacing = FacingDirection.uninitialized;

    public TRNA TRNA => tRNA;

    public void FollowCharacterAnchor(Transform followTarget)
    {
        freeFloat.enabled = false;
        behaviourChangingPuzzlePiece_Handler.DisableAll();
        rootObject.gameObject.layer = LayerReferences.NonInteractableLayer;

        targetToFollow = followTarget;
        lerpScaleRoutine = StartCoroutine(LerpToTargetScale(characterScale));
        followRoutine = StartCoroutine(FollowTarget());

        glowSpriteRenderer.enabled = false;
        AudioController.Instance.PlaySoundEffect(onCollect_Sound, false);
        ObjectiveUpdaterEvents.OnObjectiveCompleted(objectiveID);
    }

    public void ReleaseFromCharacter()
    {
        rotationTween.Kill();

        StopCoroutine(followRoutine);
        lerpScaleRoutine = StartCoroutine(LerpToTargetScale(puzzleScale));
        lerpRotationRoutine = StartCoroutine(LerpToTargetRotation());
    }

    private IEnumerator LerpToTargetScale(Vector3 targetScale)
    {
        float duration = timeToScale;
        float time = 0f;
        float normalTime = 0;
        Vector3 startingScale = rootObject.localScale;

        while (time < duration)
        {
            yield return null;

            time += Time.deltaTime;
            normalTime = Mathf.Clamp01(time / duration);

            rootObject.localScale = Vector3.Lerp(startingScale, targetScale, normalTime);
        }

        rootObject.localScale = Vector3.Lerp(startingScale, targetScale, 1);
        freeFloat.enabled = true;

        lerpScaleRoutine = null;
    }

    private IEnumerator LerpToTargetRotation()
    {
        rootObject.rotation = Quaternion.identity * Quaternion.Euler(0, 0, rootObject.rotation.z);
        Vector3 targetRotation = new Vector3(0,0, zPuzzleOffset);

        rotationTween = rootObject.DORotate(targetRotation, timeToScale, RotateMode.Fast);
        rotationTween.Play();

        while (rotationTween.IsPlaying())
        {
            yield return null;
        }

        rotationTween.Kill(true);
        freeFloat.enabled = true;

        lerpRotationRoutine = null;
    }

    private IEnumerator FollowTarget()
    {
        float modifiedSpeed;

        while (true)
        {
            yield return null;

            modifiedSpeed = followSpeed * Time.deltaTime * Mathf.Clamp01(Vector3.Distance(rootObject.position, targetToFollow.position) / followEasingDistance);
            rootObject.position = Vector3.MoveTowards(rootObject.position, targetToFollow.position, modifiedSpeed);

            UpdateRotation(characterRotationController.FacingDirection);
        }
    }

    private void UpdateRotation(FacingDirection facingDirection)
    {
        bool updateTween = false;

        float yAngle = facingDirection == FacingDirection.left ? 180 : 0;

        if (facingDirection != lastFacing)
        {
            rootObject.rotation = Quaternion.identity * Quaternion.Euler(0, yAngle, rootObject.rotation.z);
            lastFacing = facingDirection;
            updateTween = true;
        }

        if(IsOverToleranceRange(rotationTweenTargetZ,characterRotationController.TargetRotation.eulerAngles.z))
        {
            updateTween = true;
        }

        if (updateTween == true)
        {
            rotationTween.Kill();

            rotationTweenTargetZ = characterRotationController.TargetRotation.eulerAngles.z;

            Vector3 rotationTarget = new Vector3(0, yAngle, rotationTweenTargetZ);
            float duration = Mathf.Abs(rotationTweenTargetZ - rootObject.rotation.eulerAngles.z) / rotationSpeed;

            rotationTween = rootObject.DORotate(rotationTarget, duration, RotateMode.Fast);
            rotationTween.Play();
        }
    }

    private bool IsOverToleranceRange(float target, float current)
    {
        while(target > 180)
        {
            target -= 360;
        }
        while(target < -180)
        {
            target += 360;
        }

        while (current > 180)
        {
            current -= 360;
        }
        while (current < -180)
        {
            current += 360;
        }

        float result = target - current;
        while (result > 180)
        {
            result -= 360;
        }
        while (result < -180)
        {
            result += 360;
        }

        return Mathf.Abs(result) > updateTolerance;
    }

    public void SetDraggable()
    {
        behaviourChangingPuzzlePiece_Handler.SetDraggable();
    }

    private void OnDestroy()
    {
        if(rotationTween != null)
        {
            rotationTween.Kill();
        }
    }
}
