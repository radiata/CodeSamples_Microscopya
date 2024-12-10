using UnityEditor;
using UnityEngine;

public class NavigationObjectTool_SettingsProvider : SettingsProvider
{
    internal const string ToolDirectory = "Tools/Navigation Object Tool";

    private const string NavigationArea_DebugMaterial_Name = "NavigationArea_DebugMaterial";
    private const string InteractionArea_DebugMaterial_Name = "InteractionArea_DebugMaterial";

    private const string NavigationArea_DebugMaterialColor_PrefsKey = "NavigationObjectTool.NavigationArea_DebugMaterialColor";
    private const string InteractionArea_DebugMaterialColor_PrefsKey = "NavigationObjectTool.InteractionArea_DebugMaterialColor";

    internal static Color NavigationArea_DebugMaterialColor
    {
        get
        {
            return CustomEditorUtilities.GetColorFromPreferencesKey(NavigationArea_DebugMaterialColor_PrefsKey);
        }
        set
        {
            CustomEditorUtilities.SetColorToPreferencesKey(NavigationArea_DebugMaterialColor_PrefsKey, value);
            SetNavigationDebugMaterialColor(!NavigationObjectToolMenu.enableDebugMaterials);
        }
    }

    internal static Color InteractionArea_DebugMaterialColor
    {
        get
        {
            return CustomEditorUtilities.GetColorFromPreferencesKey(InteractionArea_DebugMaterialColor_PrefsKey);
        }
        set
        {
            CustomEditorUtilities.SetColorToPreferencesKey(InteractionArea_DebugMaterialColor_PrefsKey, value);
            SetInteractionDebugMaterialColor(!NavigationObjectToolMenu.enableDebugMaterials);
        }
    }

    public NavigationObjectTool_SettingsProvider(string path, SettingsScope scopes)
        : base(path, scopes)
    { }

    public override void OnGUI(string searchContext)
    {
        base.OnGUI(searchContext);

        GUILayout.Space(20f);

        EditorGUILayout.LabelField("Debug Materials");

        GUILayout.Space(5f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Navigation Debug Color");
        NavigationArea_DebugMaterialColor = EditorGUILayout.ColorField(NavigationArea_DebugMaterialColor);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Interaction Debug Color");
        InteractionArea_DebugMaterialColor = EditorGUILayout.ColorField(InteractionArea_DebugMaterialColor);
        EditorGUILayout.EndHorizontal();
    }

    [SettingsProvider]
    public static SettingsProvider CreateSettingsProvider()
    {
        return new NavigationObjectTool_SettingsProvider(ToolDirectory, SettingsScope.User);
    }

    internal static void SetNavigationDebugMaterialColor(bool clear = false)
    {
        var result = AssetDatabase.FindAssets(NavigationArea_DebugMaterial_Name);

        if (result == null)
        {
            return;
        }

        var material = (Material)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(result[0]), typeof(Material));

        if (clear)
        {
            material.color = Color.clear;
        }
        else
        {
            material.color = NavigationArea_DebugMaterialColor;
        }
    }
    internal static void SetInteractionDebugMaterialColor(bool clear = false)
    {
        var result = AssetDatabase.FindAssets(InteractionArea_DebugMaterial_Name);

        if (result == null)
        {
            return;
        }

        var material = (Material)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(result[0]), typeof(Material));

        if (clear)
        {
            material.color = Color.clear;
        }
        else
        {
            material.color = InteractionArea_DebugMaterialColor;
        }
    }
}
