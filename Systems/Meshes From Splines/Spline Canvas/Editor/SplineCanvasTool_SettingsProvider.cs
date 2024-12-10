using UnityEditor;
using UnityEngine;

public class SplineCanvasTool_SettingsProvider : SettingsProvider
{
    internal static bool SplineCanvasLockZ
    {
        get
        {
            return EditorPrefs.GetBool(SplineCanvasTool_Data.SplineCanvasLockZ_PrefsKey, true);
        }
        set
        {
            EditorPrefs.SetBool(SplineCanvasTool_Data.SplineCanvasLockZ_PrefsKey, value);
        }
    }
    internal static float SplineCanvasZPosition
    {
        get
        {
            return EditorPrefs.GetFloat(SplineCanvasTool_Data.SplineCanvasZPosition_PrefsKey, 0f);
        }
        set
        {
            EditorPrefs.SetFloat(SplineCanvasTool_Data.SplineCanvasZPosition_PrefsKey, value);
        }
    }
    internal static Color SplineCanvasMaterialColor
    {
        get
        {
            return CustomEditorUtilities.GetColorFromPreferencesKey(SplineCanvasTool_Data.SplineCanvasMaterialColor_PrefsKey);
        }
        set
        {
            CustomEditorUtilities.SetColorToPreferencesKey(SplineCanvasTool_Data.SplineCanvasMaterialColor_PrefsKey, value);
            CheckSplineCanvasColor();
        }
    }

    public SplineCanvasTool_SettingsProvider(string path, SettingsScope scopes)
        : base(path, scopes)
    { }

    [SettingsProvider]
    public static SettingsProvider CreateSettingsProvider()
    {
        return new SplineCanvasTool_SettingsProvider(SplineCanvasTool_Data.SplineCanvasTool_ToolDirectory, SettingsScope.User);
    }
    
    public override void OnGUI(string searchContext)
    {
        base.OnGUI(searchContext);

        GUILayout.Space(20f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Lock Position Z");
        SplineCanvasLockZ = EditorGUILayout.Toggle(SplineCanvasLockZ);
        GUI.enabled = SplineCanvasLockZ;
        SplineCanvasZPosition = EditorGUILayout.FloatField(SplineCanvasZPosition);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Spline Canvas Color");
        SplineCanvasMaterialColor = EditorGUILayout.ColorField(SplineCanvasMaterialColor);
        EditorGUILayout.EndHorizontal();
    }

    private static void CheckSplineCanvasColor()
    {
        if (SplineCanvasTool.activePinToViewport == null)
        {
            return;
        }

        SplineCanvasTool.activePinToViewport.SetMaterialColor(SplineCanvasMaterialColor);
    }
}
