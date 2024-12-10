using UnityEditor;
using UnityEngine;

public class NavigationObject_Setup_EditorData
{
    internal SerializedProperty corePathSpline;
    internal const string CorePathSplineName = "Core Navigation Path";
    internal GUIContent corePathSpline_Label = new GUIContent("Core Navigation Path");

    internal SerializedProperty navigationSplineBounds;
    internal SerializedProperty upperNavigationSpline;
    internal const string UpperNavigationSplineName = "Upper Navigation Bounds";
    internal SerializedProperty lowerNavigationSpline;
    internal const string LowerNavigationSplineName = "Lower Navigation Bounds";
    internal SerializedProperty upperNavigationWidth;
    internal GUIContent upperNavigationWidth_Label = new GUIContent("Upper Width");
    internal SerializedProperty lowerNavigationWidth;
    internal GUIContent lowerNavigationWidth_Label = new GUIContent("Lower Width");

    internal SerializedProperty interactionSplineBounds;
    internal SerializedProperty upperInteractionSpline;
    internal const string UpperInteractionSplineName = "Upper Interaction Bounds";
    internal SerializedProperty lowerInteractionSpline;
    internal const string LowerInteractionSplineName = "Lower Interaction Bounds";
    internal SerializedProperty upperInteractionWidth;
    internal GUIContent upperInteractionWidth_Label = new GUIContent("Upper Width");
    internal SerializedProperty lowerInteractionWidth;
    internal GUIContent lowerInteractionWidth_Label = new GUIContent("Lower Width");

    internal SerializedProperty navigationSplineMeshBuilder;
    internal GUIContent navigationSplineMeshBuilder_Label = new GUIContent("Navigation Spline Mesh Builder");
    internal SerializedProperty navigationMeshResolution;
    internal GUIContent navigationMeshResolution_Label = new GUIContent("Navigation");

    internal SerializedProperty interactionSplineMeshBuilder;
    internal GUIContent interactionSplineMeshBuilder_Label = new GUIContent("Interaction Spline Mesh Builder");
    internal SerializedProperty interactionMeshResolution;
    internal GUIContent interactionMeshResolution_Label = new GUIContent("Interaction");

    internal SerializedProperty navigationTemplateGameObject;
    internal GUIContent navigationTemplateGameObject_Label = new GUIContent("Navigation Template GameObject");

    internal SerializedProperty interactionTemplateGameObject;
    internal GUIContent interactionTemplateGameObject_Label = new GUIContent("Interaction Template GameObject");

    internal const string NewPathSplineFromCore_ButtonText = "New From Core Path";
    internal const string NewPathSplineEmpty_ButtonText = "New Empty Spline";
    internal const string EditPathSpline_ButtonText = "Edit Spline Path";

    internal const string PathBoundsFoldout_LabelText = "Path Bounds";
    internal static bool pathBoundsFoldOut_State = false;

    internal const string InteractionBoundsFoldout_LabelText = "Intraction Bounds";
    internal static bool interactionBoundsFoldOut_State = false;

    internal const string AdvancedFoldout_LabelText = "Advanced";
    internal static bool advancedFoldOut_State = false;

    internal const string Preferences_ButtonText = "Open Tool Preferences...";
}
