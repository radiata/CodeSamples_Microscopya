using UnityEditor;
using UnityEngine;

public class SplineCanvasTool_Menu : MonoBehaviour
{
    [MenuItem(SplineCanvasTool_Data.SplineCanvasTool_MenuFolder + SplineCanvasTool_Data.SplineCanvasTool_ToggleSplineCanvasCommand, priority = 12)]
    public static void ToggleSplineCanvas()
    {
        if (SplineCanvasTool.activePinToViewport == null)
        {
            SplineCanvasTool.CreateSplineCanvas(false);
        }
        else if (SplineCanvasTool.activePinToViewport != null)
        {
            GameObject.DestroyImmediate(SplineCanvasTool.activePinToViewport.gameObject);
        }

        UpdateToggleSplineCanvasCheckMark();
    }

    [MenuItem(SplineCanvasTool_Data.SplineCanvasTool_MenuFolder + SplineCanvasTool_Data.SplineCanvasTool_ClearAllSplineCanvasesCommand)]
    public static void ClearAllSplineCanvases()
    {
        PinToViewport[] items = GameObject.FindObjectsOfType<PinToViewport>();

        for (int i = 0; i < items.Length; i++)
        {
            GameObject.DestroyImmediate(items[i].gameObject);
        }

        UpdateToggleSplineCanvasCheckMark();
    }

    [MenuItem(SplineCanvasTool_Data.SplineCanvasTool_MenuFolder + SplineCanvasTool_Data.SplineCanvasTool_ToolPreferencesCommand)]
    public static void OpenToolPreferences()
    {
        SettingsService.OpenUserPreferences(SplineCanvasTool_Data.SplineCanvasTool_ToolDirectory);
    }

    public static void UpdateToggleSplineCanvasCheckMark()
    {
        Menu.SetChecked(SplineCanvasTool_Data.SplineCanvasTool_MenuFolder + SplineCanvasTool_Data.SplineCanvasTool_ToggleSplineCanvasCommand,
            SplineCanvasTool.isActivePinToViewport);
    }
}
