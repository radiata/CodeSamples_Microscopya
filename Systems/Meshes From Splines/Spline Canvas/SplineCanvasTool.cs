using UnityEditor;
using UnityEngine;

public static class SplineCanvasTool
{
    internal static PinToViewport activePinToViewport = null;
    public static bool isActivePinToViewport => activePinToViewport != null;

    public static void CreateSplineCanvas(bool destroyOnToolSwitch, bool focusSceneView = true)
    {
        if (activePinToViewport)
        {
            return;
        }

        var splineCanvasMaterial = new Material(Shader.Find("UI/Unlit/Transparent"));
        splineCanvasMaterial.color = SplineCanvasTool_SettingsProvider.SplineCanvasMaterialColor;

        var newCanvasPlane = new GameObject();
        activePinToViewport = newCanvasPlane.AddComponent<PinToViewport>();

        float? zValue = SplineCanvasTool_SettingsProvider.SplineCanvasLockZ ? SplineCanvasTool_SettingsProvider.SplineCanvasZPosition : null;
        activePinToViewport.Activate(splineCanvasMaterial, destroyOnToolSwitch, zValue);

        if(focusSceneView)
        {
            SceneView.lastActiveSceneView.Focus();
        }
    }
}
