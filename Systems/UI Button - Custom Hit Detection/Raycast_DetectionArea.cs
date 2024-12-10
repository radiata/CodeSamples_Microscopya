using UnityEngine;

public class Raycast_DetectionArea : MonoBehaviour, ICanvasRaycastFilter
{
    [SerializeField] private Camera cachedCamera;
    [SerializeField] private bool invertIsValid = false;

    [SerializeField] private RectTransform detectionRadiusCenter;
    [SerializeField] private RectTransform detectionRadiusEdge;

    private void OnEnable()
    {
        cachedCamera = CameraManager.Instance.ActiveCamera;
        CameraManager.OnCameraChanged += OnCameraChanged;
    }

    private void OnDisable()
    {
        CameraManager.OnCameraChanged -= OnCameraChanged;
    }

    private void OnCameraChanged()
    {
        cachedCamera = CameraManager.Instance.ActiveCamera;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (IsWithinRadius(sp) != invertIsValid)
        {
            return true;
        }

        return false;
    }

    private bool IsWithinRadius(Vector2 targetViewportPosition)
    {
        Vector3 screenCenter = detectionRadiusCenter.position;
        Vector3 screenEdge = detectionRadiusEdge.position;

        float detectionRadius = Vector2.Distance(screenCenter, screenEdge);

        float x = (targetViewportPosition.x - screenCenter.x);
        float y = (targetViewportPosition.y - screenCenter.y);

        return (x * x) + (y * y) <= (detectionRadius * detectionRadius);
    }
}
