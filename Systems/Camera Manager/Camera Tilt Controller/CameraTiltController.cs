using Cinemachine;
using UnityEngine;

public class CameraTiltController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private Transform rotationTarget;

    [SerializeField] private float tiltSmoothingSpeed;
    [SerializeField] private float tiltWeight;

    private float currentTiltSmoothingSpeed;
    private NavigationObjectCameraData currentCameraData;

    protected float rotationModifier = 1f;
    private bool tiltDisabled = false;

    private float tiltAngle;
    private float secondaryWeightScaler;

    private bool executeCatchUp = false;
    private float catchUpThreshold = .1f;

    public void SetControllerValues(float newTiltSmoothingSpeed, float newTiltWeight)
    {
        tiltSmoothingSpeed = newTiltSmoothingSpeed;
        tiltWeight = newTiltWeight;
        currentTiltSmoothingSpeed = tiltSmoothingSpeed;
    }

    protected virtual void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        tiltAngle = 0f;

        if (CameraTiltControl_AccessibilitySetting.tiltDisabled == false && tiltDisabled == false)
        {
            if (tiltAngle >= 0 && tiltAngle <= 180)
            {
                secondaryWeightScaler = tiltAngle / 180;
            }
            else if (tiltAngle > 180 && tiltAngle <= 360)
            {
                secondaryWeightScaler = 1 - ((tiltAngle - 180) / 180);
            }
            secondaryWeightScaler = Mathf.Clamp01(secondaryWeightScaler);

            tiltAngle = rotationTarget.eulerAngles.z;
            tiltAngle *= rotationModifier;
            tiltAngle -= tiltAngle * (1 - tiltWeight) * secondaryWeightScaler;
        }

        ApplyCameraRestrictions();

        tiltAngle = RealignAngle(tiltAngle);
        cinemachineVirtualCamera.m_Lens.Dutch = Mathf.LerpAngle(cinemachineVirtualCamera.m_Lens.Dutch, tiltAngle, currentTiltSmoothingSpeed * Time.deltaTime);
        cinemachineVirtualCamera.m_Lens.Dutch = RealignAngle(cinemachineVirtualCamera.m_Lens.Dutch);
    }

    private float RealignAngle(float angle)
    {
        while (angle > 180)
        {
            angle = angle - 360;
        }
        while (angle < -180)
        {
            angle = angle + 360;
        }
        return angle;
    }

    private void ApplyCameraRestrictions()
    {
        if (currentCameraData.UseCameraData == false)
        {
            return;
        }

        if (currentCameraData.ClampCameraAngle == true)
        {
            tiltAngle = Mathf.LerpAngle(cinemachineVirtualCamera.m_Lens.Dutch, tiltAngle, 1);
            tiltAngle = Mathf.Clamp(tiltAngle, currentCameraData.AngleRange.x, currentCameraData.AngleRange.y);
        }

        if (currentCameraData.CatchUpAngle == true)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(cinemachineVirtualCamera.m_Lens.Dutch, tiltAngle)) >= currentCameraData.AngleDifference)
            {
                executeCatchUp = true;
            }
            else
            {
                currentTiltSmoothingSpeed = currentCameraData.BaseSmoothingSpeed;
            }
        }

        if (executeCatchUp == true)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(cinemachineVirtualCamera.m_Lens.Dutch, tiltAngle)) <= catchUpThreshold)
            {
                executeCatchUp = false;
            }
            currentTiltSmoothingSpeed = currentCameraData.MaxSmoothingSpeed;
        }
    }

    private void OnNavigationObjectChanged(NavigationObject navigationObject)
    {
        executeCatchUp = false;
        currentTiltSmoothingSpeed = tiltSmoothingSpeed;
        currentCameraData = navigationObject.NavigationObjectCameraData;
    }

    protected virtual void OnEnable()
    {
        CharacterNavigationObjectReporter.OnNavigationObjectChanged += OnNavigationObjectChanged;
    }

    protected virtual void OnDisable()
    {
        CharacterNavigationObjectReporter.OnNavigationObjectChanged -= OnNavigationObjectChanged;
    }
}
