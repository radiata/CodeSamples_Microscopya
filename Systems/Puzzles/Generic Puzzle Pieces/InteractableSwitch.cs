using System.Collections;
using UnityEngine;

public class InteractableSwitch : MonoBehaviour, I_SwitchPuzzlePiece
{
    [SerializeField] private SwitchResponder_Reference switchResponder_Reference;
    private I_SwitchResponder switchResponder;
    [SerializeField] private bool updateSwitchResponder;

    [SerializeField] private bool initializeOnStart = false;
    [SerializeField] private SwitchState defaultState = SwitchState.Off;

    [SerializeField] private bool lerpSwitchPosition;
    [SerializeField] private float lerpPositionSeconds;

    [SerializeField] private bool lerpSwitchRotation;
    [SerializeField] private float lerpRotationSeconds;

    [SerializeField] private bool lerpSwitchScale;
    [SerializeField] private float lerpScaleSeconds;


    [Header("On Transform")]
    [SerializeField] private Vector3 onPosition_Local = Vector3.zero;
    [SerializeField] private Vector3 onRotation_Local = Vector3.zero;
    [SerializeField] private Vector3 onScale_Local = Vector3.one;

    [Header("Off Transform")]
    [SerializeField] private Vector3 offPosition_Local = Vector3.zero;
    [SerializeField] private Vector3 offRotation_Local = Vector3.zero;
    [SerializeField] private Vector3 offScale_Local = Vector3.one;

    [Header("Navigation Settings")]
    [SerializeField] private bool navigateToPuzzleOnInteract = true;
    [SerializeField] private PuzzleManager puzzleManager;

    [Header("Sound Settings")]
    [SerializeField] private SoundEffect toggle_Sound;
    [SerializeField] private SoundEffect whileInteracting_Sound;

    private SwitchState switchState;
    private Coroutine switchAnimationRoutine;

    public SwitchState SwitchState => switchState;

    public void InitializeState(SwitchState initialState)
    {
        if (lerpSwitchPosition == false)
        {
            onPosition_Local = transform.localPosition;
            offPosition_Local = transform.localPosition;
        }

        if (lerpSwitchRotation == false)
        {
            onRotation_Local = transform.localRotation.eulerAngles;
            offRotation_Local = transform.localRotation.eulerAngles;
        }

        if (lerpSwitchScale == false)
        {
            onScale_Local = transform.localScale;
            offScale_Local = transform.localScale;
        }

        switchState = initialState;

        if (switchState == SwitchState.On)
        {
            transform.localPosition = onPosition_Local;
            transform.localRotation = Quaternion.Euler(onRotation_Local);
            transform.localScale = onScale_Local;
        }

        if (switchState == SwitchState.Off)
        {
            transform.localPosition = offPosition_Local;
            transform.localRotation = Quaternion.Euler(offRotation_Local);
            transform.localScale = offScale_Local;
        }

        if (switchState == SwitchState.Busy)
        {
            Debug.LogWarning("Initialized to busy state");
        }
    }

    public void SwitchToggle()
    {
        if (navigateToPuzzleOnInteract)
        {
            puzzleManager.Navigate();
        }

        if (switchState == SwitchState.Busy)
        {
            return;
        }

        if (switchAnimationRoutine != null)
        {
            StopCoroutine(switchAnimationRoutine);
        }

        AudioController.Instance.PlaySoundEffect(toggle_Sound, false);

        bool targetOnState = switchState == SwitchState.Off;
        switchAnimationRoutine = StartCoroutine(SwitchAnimation(targetOnState));
    }

    public void OnDragStart(Vector3 worldPosition)
    {
        SwitchToggle();
    }

    public void WhileDragging(Vector3 worldPosition, Vector3 cameraForward)
    { }

    public void OnDragEnd(Vector3 worldPosition)
    { }

    private void Start()
    {
        switchResponder = switchResponder_Reference.GetSwitchResponder();
        if (initializeOnStart == true)
        {
            InitializeState(defaultState);
        }
    }

    private IEnumerator SwitchAnimation(bool isOn_TargetState)
    {
        switchState = SwitchState.Busy;

        if (updateSwitchResponder == true)
        {
            switchResponder.SwitchIsBusy();
        }

        (Vector3, Vector3) lerpPositions = isOn_TargetState ? (offPosition_Local, onPosition_Local) : (onPosition_Local, offPosition_Local);
        (Vector3, Vector3) lerpRotations = isOn_TargetState ? (offRotation_Local, onRotation_Local) : (onRotation_Local, offRotation_Local);
        (Vector3, Vector3) lerpScales = isOn_TargetState ? (offScale_Local, onScale_Local) : (onScale_Local, offScale_Local);

        float elapsedTime = 0;
        bool positionComplete = false;
        bool rotationComplete = false;
        bool scaleComplete = false;

        while (positionComplete == false || rotationComplete == false || scaleComplete == false)
        {
            positionComplete = positionComplete ? true : LerpPosition(elapsedTime, lerpPositions.Item1, lerpPositions.Item2);
            rotationComplete = rotationComplete ? true : LerpRotation(elapsedTime, lerpRotations.Item1, lerpRotations.Item2);
            scaleComplete = scaleComplete ? true : LerpScale(elapsedTime, lerpScales.Item1, lerpScales.Item2);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        switchState = isOn_TargetState ? SwitchState.On : SwitchState.Off;

        if (updateSwitchResponder == true)
        {
            if (isOn_TargetState == true)
            {
                switchResponder.SwitchIsOn();
            }
            else
            {
                switchResponder.SwitchIsOff();
            }
        }

        switchAnimationRoutine = null;
    }

    private bool LerpPosition(float elapsedTime, Vector3 start_Local, Vector3 end_Local)
    {
        float t;
        if (lerpSwitchPosition == false || lerpPositionSeconds <= 0)
        {
            t = 1;
        }
        else
        {
            t = Mathf.Clamp01(elapsedTime / lerpPositionSeconds);
        }

        transform.localPosition = Vector3.Lerp(start_Local, end_Local, t);

        return t >= 1;
    }

    private bool LerpRotation(float elapsedTime, Vector3 start_Local, Vector3 end_Local)
    {
        float t;
        if (lerpSwitchRotation == false || lerpRotationSeconds <= 0)
        {
            t = 1;
        }
        else
        {
            t = Mathf.Clamp01(elapsedTime / lerpRotationSeconds);
        }

        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(start_Local), Quaternion.Euler(end_Local), t);

        return t >= 1;
    }

    private bool LerpScale(float elapsedTime, Vector3 start_Local, Vector3 end_Local)
    {
        float t;
        if (lerpSwitchScale == false || lerpScaleSeconds <= 0)
        {
            t = 1;
        }
        else
        {
            t = Mathf.Clamp01(elapsedTime / lerpScaleSeconds);
        }

        transform.localScale = Vector3.Lerp(start_Local, end_Local, t);

        return t >= 1;
    }
}
