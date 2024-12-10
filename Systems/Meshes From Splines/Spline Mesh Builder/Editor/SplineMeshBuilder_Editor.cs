using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SplineMeshBuilder))]
public class SplineMeshBuilder_Editor : UnityEditor.Editor
{
    private SerializedProperty centerPath;
    private GUIContent centerPath_Label = new GUIContent("Center Path Spline");

    private SerializedProperty topBounds;
    private GUIContent topBounds_Label = new GUIContent("Top Bounds Spline");

    private SerializedProperty bottomBounds;
    private GUIContent bottomBounds_Label = new GUIContent("Bottom Bounds Spline");

    private SerializedProperty meshResolution;
    private GUIContent meshResolution_Label = new GUIContent("Mesh Resolution");

    private SerializedProperty templateGameObject;
    private GUIContent templateGameObject_Label = new GUIContent("Template GameObject");

    private SerializedProperty targetGameObjectPlacement;
    private GUIContent targetGameObjectPlacement_Label = new GUIContent("Target GameObject Placement");

    private SplineMeshBuilder targetComponent = null;
    private GUIStyle labelStyle;

    private void OnEnable()
    {
        targetComponent = (SplineMeshBuilder)target;

        centerPath = serializedObject.FindProperty(SplineMeshBuilder.GetCenterPathVariableName());
        topBounds = serializedObject.FindProperty(SplineMeshBuilder.GetTopBoundsVariableName());
        bottomBounds = serializedObject.FindProperty(SplineMeshBuilder.GetBottomBoundsVariableName());
        meshResolution = serializedObject.FindProperty(SplineMeshBuilder.GetMeshResolutionVariableName());
        templateGameObject = serializedObject.FindProperty(SplineMeshBuilder.GetTemplateGameObjectVariableName());
        targetGameObjectPlacement = serializedObject.FindProperty(SplineMeshBuilder.GetTargetPlacementVariableName());
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawHeadSection();

        DrawBuildTypeSelection();
        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        DrawContextOptions();
        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        DrawGenerateMeshButton();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawHeadSection()
    {
        if (string.IsNullOrEmpty(targetComponent.GetMeshName()) == false)
        {
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
            }

            EditorGUILayout.LabelField(targetComponent.GetMeshName(), labelStyle, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);
        }

        GUI.enabled = true;
        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);
    }

    private void DrawBuildTypeSelection()
    {
        CustomEditorUtilities.ValidatedProperty(meshResolution, meshResolution_Label,
            (object value) => { return SplineMeshBuilder.IsValidResolution((int)value); },
            (object value) => { return 1; });
    }

    private void DrawContextOptions()
    {
        EditorGUILayout.PropertyField(centerPath, centerPath_Label);
        EditorGUILayout.PropertyField(topBounds, topBounds_Label);
        EditorGUILayout.PropertyField(bottomBounds, bottomBounds_Label);
    }

    private void DrawGenerateMeshButton()
    {
        GUI.enabled = ContextRequirements();

        if (GUILayout.Button("Generate Mesh"))
        {
            BuildMesh();
        }

        GUI.enabled = true;
    }

    private void BuildMesh()
    {
        Vector2[] polygon2DVerts = SplineMeshBuilder.GetPolygon2DVerts(targetComponent.GetMeshResolution(), targetComponent.GetTopBounds().Spline, targetComponent.GetBottomBounds().Spline);
        GameObject resultGO = targetComponent.CreateGameObjectFromTemplate(targetComponent.GetTemplateGameObject(), polygon2DVerts, targetComponent.GetTargetPlacement().position);

        if (targetComponent.GetGeneratedGameObject() != null)
        {
            DestroyImmediate(targetComponent.GetGeneratedGameObject());
        }
        targetComponent.SetGeneratedGameObject(resultGO);

        NavMeshSurface navMeshSurface = resultGO.GetComponentInChildren<NavMeshSurface>();
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
    }

    private bool ContextRequirements()
    {
        if (topBounds.boxedValue == null || bottomBounds.boxedValue == null)
        {
            return false;
        }
        return true;
    }
}
