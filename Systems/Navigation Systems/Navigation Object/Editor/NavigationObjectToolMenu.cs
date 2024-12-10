using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class NavigationObjectToolMenu
{
    internal static bool enableDebugMaterials = false;

    const string NavigationObject_MenuFolder = "Tools/Navigation Object Tool/";

    const string NavigationObject_CreateCommand = "Create Navigation Object";
    const string BaseObjectName = "Navigation Object";

    const string NavigationObject_EnableDebugMaterialsCommand = "Enable Debug Materials";

    const string NavigationObject_ToolPreferencesCommand = "Tool Preferences...";

    public static bool isActiveDebugMaterials => enableDebugMaterials;

    static NavigationObjectToolMenu()
    {
        enableDebugMaterials = false;
        NavigationObjectTool_SettingsProvider.SetNavigationDebugMaterialColor(!enableDebugMaterials);
        NavigationObjectTool_SettingsProvider.SetInteractionDebugMaterialColor(!enableDebugMaterials);
    }

    [MenuItem(NavigationObject_MenuFolder + NavigationObject_CreateCommand, priority = 11)]
    public static void CreateNewNavigationObject()
    {
        GameObject newGameObject = new GameObject(NewObjectName());

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName(NavigationObject_CreateCommand);
        Undo.RegisterCreatedObjectUndo(newGameObject, NavigationObject_MenuFolder + NavigationObject_CreateCommand);

        newGameObject.AddComponent<NavigationObject_Setup>();

        Selection.activeObject = newGameObject;
    }

    [MenuItem(NavigationObject_MenuFolder + NavigationObject_ToolPreferencesCommand)]
    public static void OpenToolPreferences()
    {
        SettingsService.OpenUserPreferences(NavigationObjectTool_SettingsProvider.ToolDirectory);
    }

    [MenuItem(NavigationObject_MenuFolder + NavigationObject_EnableDebugMaterialsCommand)]
    public static void EnableDebugMaterials()
    {
        enableDebugMaterials = !enableDebugMaterials;

        NavigationObjectTool_SettingsProvider.SetNavigationDebugMaterialColor(!enableDebugMaterials);
        NavigationObjectTool_SettingsProvider.SetInteractionDebugMaterialColor(!enableDebugMaterials);

        Menu.SetChecked(NavigationObject_MenuFolder + NavigationObject_EnableDebugMaterialsCommand, enableDebugMaterials);
    }

    private static string NewObjectName()
    {
        var navigationObjects = GameObject.FindObjectsByType(typeof(NavigationObject_Setup), FindObjectsInactive.Include, FindObjectsSortMode.None);

        var currentIteration = navigationObjects.Length;

        var objectName = BaseObjectName;

        switch (EditorSettings.gameObjectNamingScheme)
        {
            case EditorSettings.NamingScheme.SpaceParenthesis:
                objectName += currentIteration <= 10 ? $" (0{currentIteration})" : $" ({currentIteration})";
                break;
            case EditorSettings.NamingScheme.Dot:
                objectName += currentIteration <= 10 ? $" .0{currentIteration}" : $".{currentIteration}";
                break;
            case EditorSettings.NamingScheme.Underscore:
                objectName += currentIteration <= 10 ? $" _0{currentIteration}" : $"_{currentIteration}";
                break;
        }

        return objectName;
    }
}
