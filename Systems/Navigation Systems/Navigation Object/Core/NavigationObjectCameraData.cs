using UnityEngine;

[System.Serializable]
public struct NavigationObjectCameraData
{
    [SerializeField] public bool UseCameraData;

    [SerializeField] public bool ClampCameraAngle;
    [SerializeField] public Vector2 AngleRange;

    [SerializeField] public bool CatchUpAngle;
    [SerializeField] public float AngleDifference;
    [SerializeField] public float BaseSmoothingSpeed;
    [SerializeField] public float MaxSmoothingSpeed;
}
