using UnityEngine;

public class SynthaseWheel : MonoBehaviour, I_RotationResponder
{
    //drag to rotate
    //when rotated far enough in a direction set complete
    //when locked (aka not yet solvable) do not allow rotation past a point
    //on release reset rotation

    [SerializeField] private float baseRotation = 0f;
    [SerializeField] private float rotationDeltaWhileLocked = 20f;
    [SerializeField] private float rotationDeltaToSolve = 100f;

    [SerializeField] private bool rotationLocked = true;

    private float rotationMaxLocked;
    private float rotationMinLocked;
    private float rotationSolutionThresholdUpper;
    private float rotationSolutionThresholdLower;

    private bool solved = false;
    public bool isSolved => solved;

    public delegate void WheelUpdateEvent();
    public event WheelUpdateEvent OnWheelUpdated;

    public void SetSolved()
    {
        solved = true;
        OnWheelUpdated?.Invoke();
    }

    public void UnlockWheel()
    {
        rotationLocked = false;
    }

    public void StartRotation(float initialRotation)
    { }

    public void UpdateRotation(float currentRotation)
    {
        if (solved)
        {
            return;
        }

        if (rotationLocked)
        {
            LimitRotation(currentRotation);
            return;
        }

        CheckSolution(currentRotation);
    }

    public bool? EndRotation(float finalRotation)
    {
        if (solved)
        {
            return null;
        }

        CheckSolution(finalRotation);
        ResetRotation();
        return null;
    }

    private void LimitRotation(float currentRotation)
    {
        currentRotation = ConvertFrom360to180(currentRotation);
        if (currentRotation > rotationMaxLocked)
        {
            transform.rotation = Quaternion.AngleAxis(rotationMaxLocked, Vector3.forward);
            return;
        }

        if (currentRotation < rotationMinLocked)
        {
            transform.rotation = Quaternion.AngleAxis(rotationMinLocked, Vector3.forward);
            return;
        }
    }

    private void CheckSolution(float currentRotation)
    {
        currentRotation = ConvertFrom360to180(currentRotation);
        if (currentRotation >= rotationSolutionThresholdUpper
            || currentRotation <= rotationSolutionThresholdLower)
        {
            SetSolved();
        }
    }

    private float ConvertFrom360to180(float currentRotation)
    {
        while (currentRotation > 180)
        {
            currentRotation -= 360;
        }
        while (currentRotation < -180)
        {
            currentRotation += 360;
        }

        return currentRotation;
    }

    private void ResetRotation()
    {
        transform.rotation = Quaternion.AngleAxis(baseRotation, Vector3.forward);
    }

    private void OnEnable()
    {
        rotationMaxLocked = baseRotation + rotationDeltaWhileLocked;
        rotationMinLocked = baseRotation - rotationDeltaWhileLocked;

        rotationSolutionThresholdUpper = baseRotation + rotationDeltaToSolve;
        rotationSolutionThresholdLower = baseRotation - rotationDeltaToSolve;

        ResetRotation();
    }
}
