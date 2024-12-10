using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using System.Linq;

public class CameraZoomController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float zoomSmoothingSpeed = 1f;
    [SerializeField] private float defaultZoom = 10;
    [SerializeField] private int zoomClampPriority = 0;

    private static float zoomValue = 1f;
    private static float globalDefaultZoomValue = 10f;
    private static float globalMinZoomValue = 0;
    private static float globalMaxZoomValue = 100;
    private static float lastCachedValue;

    private static float activeMaxZoomValue;
    private static float activeMinZoomValue;

    private static List<CameraZoomController> activeZoomControllers = new List<CameraZoomController>();

    [SerializeField] private float minZoomValue = 5;
    [SerializeField] private float maxZoomValue = 15;

    public static void SetZoomTo(float newZoomValue)
    {
        zoomValue = Mathf.Clamp(newZoomValue, activeMinZoomValue, activeMaxZoomValue);
    }
    public static void ChangeZoomBy(float deltaValue)
    {
        zoomValue = Mathf.Clamp(zoomValue + deltaValue, activeMinZoomValue, activeMaxZoomValue);
    }
    public static void ResetZoomToGlobalDefault()
    {
        SetZoomTo(globalDefaultZoomValue);
    }
    public static void ResetZoomToLastCachedValue()
    {
        SetZoomTo(lastCachedValue);
    }
    public static void CacheZoomValue()
    {
        lastCachedValue = zoomValue;
    }

    public void SetLocalZoomSmoothingSpeed(float newValue)
    {
        zoomSmoothingSpeed = newValue;
    }

    public void SetLocalMinimumZoomValue(float newValue)
    {
        minZoomValue = newValue;
        UpdateActiveZoomValues();
    }

    public void SetLocalMaximumZoomValue(float newValue)
    {
        maxZoomValue = newValue;
        UpdateActiveZoomValues();
    }

    public void SetDefaultZoomValue(float newValue)
    {
        defaultZoom = newValue;
    }

    public void ResetZoom()
    {
        SetZoomTo(defaultZoom);
    }

    private void Update()
    {
        if (virtualCamera.m_Lens.OrthographicSize == zoomValue)
        {
            return;
        }

        var zoomLevel = Mathf.Clamp(zoomValue, minZoomValue, maxZoomValue);
        virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(virtualCamera.m_Lens.OrthographicSize, zoomLevel, zoomSmoothingSpeed * Time.deltaTime);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
        zoomValue = globalDefaultZoomValue;
    }

    private void OnEnable()
    {
        AddActiveCameraZoomController(this);
    }

    private void OnDisable()
    {
        RemoveActiveCameraZoomController(this);
    }

    private static void AddActiveCameraZoomController(CameraZoomController cameraZoomController)
    {
        activeZoomControllers.Add(cameraZoomController);
        UpdateActiveZoomValues();
    }

    private static void RemoveActiveCameraZoomController(CameraZoomController cameraZoomController)
    {
        activeZoomControllers.Remove(cameraZoomController);
        UpdateActiveZoomValues();
    }

    private static void UpdateActiveZoomValues()
    {
        if (activeZoomControllers.Count == 0)
        {
            activeMaxZoomValue = globalMaxZoomValue;
            activeMinZoomValue = globalMinZoomValue;
            return;
        }

        activeZoomControllers.OrderByDescending(controller => controller.zoomClampPriority);

        activeMaxZoomValue = activeZoomControllers[0].maxZoomValue;
        activeMinZoomValue = activeZoomControllers[0].minZoomValue;

        if (activeZoomControllers.Count == 1)
        {
            return;
        }

        List<CameraZoomController> filteredList =
            activeZoomControllers.Where(controller => controller.zoomClampPriority == activeZoomControllers[0].zoomClampPriority).ToList();

        for (int i = 0; i < filteredList.Count; i++)
        {
            if (filteredList[i].maxZoomValue > activeMaxZoomValue)
            {
                activeMaxZoomValue = filteredList[i].maxZoomValue;
            }

            if (filteredList[i].minZoomValue < activeMinZoomValue)
            {
                activeMinZoomValue = filteredList[i].minZoomValue;
            }
        }
    }
}
