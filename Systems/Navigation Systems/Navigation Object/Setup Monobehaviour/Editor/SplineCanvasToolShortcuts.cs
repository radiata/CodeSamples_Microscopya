using UnityEditor;
using UnityEditor.Splines;
using UnityEngine;

internal static class SplineCanvasToolShortcuts
{
    internal static void DrawSplineCanvasToggleButton()
    {
        if (SplineCanvasTool.isActivePinToViewport && GUILayout.Button("Disable Spline Canvas"))
        {
            SplineCanvasTool_Menu.ToggleSplineCanvas();
        }

        if (SplineCanvasTool.isActivePinToViewport == false && GUILayout.Button("Enable Spline Canvas"))
        {
            SplineCanvasTool_Menu.ToggleSplineCanvas();
        }
    }

    public static void CreateSplineCanvas(Object selectObject)
    {
        if (selectObject != null)
        {
            Selection.activeObject = selectObject;
        }

        SplineAccess.SplineToolAccess();
        EditorApplication.delayCall += CreateSplineCanvasEventSubscriber;
    }

    private static void CreateSplineCanvasEventSubscriber()
    {
        SplineCanvasTool.CreateSplineCanvas(true);
        EditorApplication.delayCall -= CreateSplineCanvasEventSubscriber;
    }
}
