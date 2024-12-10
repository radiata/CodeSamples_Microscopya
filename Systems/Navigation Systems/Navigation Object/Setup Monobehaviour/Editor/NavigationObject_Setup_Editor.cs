using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using System;

[CustomEditor(typeof(NavigationObject_Setup))]
public class NavigationObject_Setup_Editor : UnityEditor.Editor
{
    private NavigationObject_Setup targetComponent = null;

    private NavigationObject_Setup_EditorData data = new NavigationObject_Setup_EditorData();

    private float previousValue_UpperNavigationWidth;
    private float previousValue_LowerNavigationWidth;
    private float previousValue_UpperInteractionWidth;
    private float previousValue_LowerInteractionWidth;
    private bool isDirty = false;
    private bool isSplineWidthDirty = false;
    private bool updateSplineMeshBuilders = false;

    private void OnEnable()
    {
        targetComponent = (NavigationObject_Setup)target;
        NavigationObject_Setup_EditorUtilities.AssignProperties(ref data, serializedObject, targetComponent);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        previousValue_UpperNavigationWidth = data.upperNavigationWidth.floatValue;
        previousValue_LowerNavigationWidth = data.lowerNavigationWidth.floatValue;
        previousValue_UpperInteractionWidth = data.upperInteractionWidth.floatValue;
        previousValue_LowerInteractionWidth = data.lowerInteractionWidth.floatValue;

        try
        {
            DrawCorePathSection();
            EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);
            DrawAdvancedFoldout();
            EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

            //DrawRefreshButton();
            EditorGUILayout.BeginHorizontal();
            SplineCanvasToolShortcuts.DrawSplineCanvasToggleButton();

            if (GUILayout.Button(NavigationObjectToolMenu.isActiveDebugMaterials ? "Disable Debug Materials" : "Enable Debug Materials"))
            {
                NavigationObjectToolMenu.EnableDebugMaterials();
            }

            DrawPreferencesOptions();
            EditorGUILayout.EndHorizontal();
        }
        catch (Exception exception)
        {
            if (exception.GetType() != typeof(NullReferenceException))
            {
                Debug.LogError(exception);
            }
        }

        DirtyCheck();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawCorePathSection()
    {
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(data.corePathSpline, data.corePathSpline_Label);

        DrawModifyPathButtons(ref data.corePathSpline, Vector3.zero, 0, NavigationObject_Setup_EditorData.CorePathSplineName, NavigationObject_Setup_EditorData.EditPathSpline_ButtonText, NavigationObject_Setup_EditorData.NewPathSplineEmpty_ButtonText, null);

        if (EditorGUI.EndChangeCheck())
        {
            targetComponent.SetCachedSpline(null);
            targetComponent.ForceCorePathToBezier();
            updateSplineMeshBuilders = true;
        }

        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Navigation Bounds");
        EditorGUILayout.BeginHorizontal();
        DrawValidatedBounds(data.upperNavigationWidth, data.upperNavigationWidth_Label);
        DrawValidatedBounds(data.lowerNavigationWidth, data.lowerNavigationWidth_Label);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        EditorGUILayout.LabelField("Interaction Bounds");
        EditorGUILayout.BeginHorizontal();
        DrawValidatedBounds(data.upperInteractionWidth, data.upperInteractionWidth_Label);
        DrawValidatedBounds(data.lowerInteractionWidth, data.lowerInteractionWidth_Label);
        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
        {
            isSplineWidthDirty = true;
        }

        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        if (GUILayout.Button("Generate All Bounds"))
        {
            updateSplineMeshBuilders = NavigationObject_Setup_EditorUtilities.GenerateBounds(ref data, targetComponent, true, true);
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Navigation Bounds"))
        {
            updateSplineMeshBuilders = NavigationObject_Setup_EditorUtilities.GenerateBounds(ref data, targetComponent, true, false);
        }
        if (GUILayout.Button("Generate Interaction Bounds"))
        {
            updateSplineMeshBuilders = NavigationObject_Setup_EditorUtilities.GenerateBounds(ref data, targetComponent, false, true);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Mesh Resolution");
        EditorGUILayout.BeginHorizontal();
        DrawValidatedResolution(data.navigationMeshResolution, data.navigationMeshResolution_Label);
        DrawValidatedResolution(data.interactionMeshResolution, data.interactionMeshResolution_Label);
        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
        {
            updateSplineMeshBuilders = true;
        }

        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        if (GUILayout.Button("Generate All Meshes"))
        {
            //this order is fragile, navigation first, then interaction...
            NavigationObject_Setup_EditorUtilities.GenerateMesh(targetComponent.GetNavigationSplineMeshBuilder(), targetComponent.GetCorePathSpline().transform.localPosition);
            NavigationObject_Setup_EditorUtilities.GenerateMesh(targetComponent.GetInteractionSplineMeshBuilder(), targetComponent.GetCorePathSpline().transform.localPosition);
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Navigation Mesh"))
        {
            NavigationObject_Setup_EditorUtilities.GenerateMesh(targetComponent.GetNavigationSplineMeshBuilder(), targetComponent.GetCorePathSpline().transform.localPosition);
        }

        if (GUILayout.Button("Generate Interaction Mesh"))
        {
            NavigationObject_Setup_EditorUtilities.GenerateMesh(targetComponent.GetInteractionSplineMeshBuilder(), targetComponent.GetCorePathSpline().transform.localPosition);
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawNavigationBoundsSection(bool withFoldout)
    {
        if (withFoldout == true)
        {
            NavigationObject_Setup_EditorData.pathBoundsFoldOut_State = EditorGUILayout.BeginFoldoutHeaderGroup(NavigationObject_Setup_EditorData.pathBoundsFoldOut_State, NavigationObject_Setup_EditorData.PathBoundsFoldout_LabelText);

            if (NavigationObject_Setup_EditorData.pathBoundsFoldOut_State == false)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
        }
        else
        {
            EditorGUILayout.LabelField("Navigation Bounds", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
        }

        EditorGUILayout.LabelField("Upper Bounds");
        DrawModifyPathButtons(ref data.upperNavigationSpline, Vector3.right, targetComponent.GetNavigationUpperWidth(), NavigationObject_Setup_EditorData.UpperNavigationSplineName, NavigationObject_Setup_EditorData.EditPathSpline_ButtonText, NavigationObject_Setup_EditorData.NewPathSplineEmpty_ButtonText, NavigationObject_Setup_EditorData.NewPathSplineFromCore_ButtonText);

        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        EditorGUILayout.LabelField("Lower Bounds");
        DrawModifyPathButtons(ref data.lowerNavigationSpline, Vector3.left, targetComponent.GetNavigationLowerWidth(), NavigationObject_Setup_EditorData.LowerNavigationSplineName, NavigationObject_Setup_EditorData.EditPathSpline_ButtonText, NavigationObject_Setup_EditorData.NewPathSplineEmpty_ButtonText, NavigationObject_Setup_EditorData.NewPathSplineFromCore_ButtonText);


        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(data.navigationSplineBounds);

        if (EditorGUI.EndChangeCheck())
        {
            updateSplineMeshBuilders = true;
            isSplineWidthDirty = true;
        }

        if (withFoldout == true)
        {
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        else
        {
            EditorGUI.indentLevel--;
        }
    }

    private void DrawInteractionBoundsSection(bool withFoldout)
    {
        if (withFoldout == true)
        {
            NavigationObject_Setup_EditorData.interactionBoundsFoldOut_State = EditorGUILayout.BeginFoldoutHeaderGroup(NavigationObject_Setup_EditorData.interactionBoundsFoldOut_State, NavigationObject_Setup_EditorData.InteractionBoundsFoldout_LabelText);

            if (NavigationObject_Setup_EditorData.interactionBoundsFoldOut_State == false)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
        }
        else
        {
            EditorGUILayout.LabelField("Interaction Bounds", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
        }

        EditorGUILayout.LabelField("Upper Bounds");
        DrawModifyPathButtons(ref data.upperInteractionSpline, Vector3.right, targetComponent.GetInteractionUpperWidth(), NavigationObject_Setup_EditorData.UpperInteractionSplineName, NavigationObject_Setup_EditorData.EditPathSpline_ButtonText, NavigationObject_Setup_EditorData.NewPathSplineEmpty_ButtonText, NavigationObject_Setup_EditorData.NewPathSplineFromCore_ButtonText);

        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        EditorGUILayout.LabelField("Lower Bounds");
        DrawModifyPathButtons(ref data.lowerInteractionSpline, Vector3.left, targetComponent.GetInteractionLowerWidth(), NavigationObject_Setup_EditorData.LowerInteractionSplineName, NavigationObject_Setup_EditorData.EditPathSpline_ButtonText, NavigationObject_Setup_EditorData.NewPathSplineEmpty_ButtonText, NavigationObject_Setup_EditorData.NewPathSplineFromCore_ButtonText);


        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(data.interactionSplineBounds);

        if (EditorGUI.EndChangeCheck())
        {
            updateSplineMeshBuilders = true;
            isSplineWidthDirty = true;
        }

        if (withFoldout == true)
        {
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        else
        {
            EditorGUI.indentLevel--;
        }
    }

    private void DrawModifyPathButtons(ref SerializedProperty serializedProperty, Vector3 splineOffsetDirection, float offsetDistance, string newPathName, string editSpline_ButtonText, string createEmptySpline_ButtonText, string createSplineFromCore_ButtonText = null)
    {
        EditorGUILayout.BeginHorizontal();

        if (serializedProperty.boxedValue != null)
        {
            if (GUILayout.Button(editSpline_ButtonText))
            {
                if (serializedProperty == data.corePathSpline)
                {
                    targetComponent.SetCachedSpline(targetComponent.GetCorePathSpline().Spline);
                }

                SplineCanvasToolShortcuts.CreateSplineCanvas((SplineContainer)serializedProperty.boxedValue);
                return;
            }
        }

        if (createSplineFromCore_ButtonText != null && data.corePathSpline != null)
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(createSplineFromCore_ButtonText))
            {
                NavigationObject_Setup_EditorUtilities.CreateNewSpline(serializedProperty, true, newPathName, createSplineFromCore_ButtonText, splineOffsetDirection, offsetDistance, targetComponent);
                updateSplineMeshBuilders = true;
                return;
            }
        }

        if (GUILayout.Button(createEmptySpline_ButtonText))
        {
            SplineContainer newSpline = NavigationObject_Setup_EditorUtilities.CreateNewSpline(serializedProperty, false, newPathName, createEmptySpline_ButtonText, splineOffsetDirection, offsetDistance, targetComponent);
            SplineCanvasToolShortcuts.CreateSplineCanvas(newSpline);
            updateSplineMeshBuilders = true;
            return;
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawPreferencesOptions()
    {
        if (GUILayout.Button(NavigationObject_Setup_EditorData.Preferences_ButtonText))
        {
            NavigationObjectToolMenu.OpenToolPreferences();
        }
    }

    private void DrawAdvancedFoldout()
    {
        NavigationObject_Setup_EditorData.advancedFoldOut_State = EditorGUILayout.BeginFoldoutHeaderGroup(NavigationObject_Setup_EditorData.advancedFoldOut_State, NavigationObject_Setup_EditorData.AdvancedFoldout_LabelText);

        if (NavigationObject_Setup_EditorData.advancedFoldOut_State)
        {
            DrawNavigationBoundsSection(false);
            EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

            DrawInteractionBoundsSection(false);
            EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

            DrawSplineMeshBuilderGroup();
            DrawTemplateSection();
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void DrawSplineMeshBuilderGroup()
    {
        GUI.enabled = false;

        EditorGUILayout.PropertyField(data.navigationSplineMeshBuilder, data.navigationSplineMeshBuilder_Label);
        EditorGUILayout.PropertyField(data.interactionSplineMeshBuilder, data.interactionSplineMeshBuilder_Label);

        GUI.enabled = true;
    }

    private void DrawRefreshButton()
    {
        if (targetComponent.GetCachedSpline() == null)
        {
            EditorGUILayout.LabelField("Warning: Core Spline is not cached. Press Refresh splines before modifying Core Navigation Path's Spline.", EditorStyles.helpBox);
        }
        if (GUILayout.Button("Refresh Splines"))
        {
            isDirty = true;
        }
    }

    private void DrawTemplateSection()
    {
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(data.navigationTemplateGameObject, data.navigationTemplateGameObject_Label);
        EditorGUILayout.PropertyField(data.interactionTemplateGameObject, data.interactionTemplateGameObject_Label);

        if (EditorGUI.EndChangeCheck())
        {
            updateSplineMeshBuilders = true;
        }
    }

    private void DrawValidatedBounds(SerializedProperty property, GUIContent label)
    {
        CustomEditorUtilities.ValidatedProperty(property, label,
            (object value) => { return SplineBounds.IsValidWidth((float)value); },
            (object value) => { return SplineBounds.MINIMUM_WIDTH; });
    }

    private void DrawValidatedResolution(SerializedProperty property, GUIContent label)
    {
        CustomEditorUtilities.ValidatedProperty(property, label,
            (object value) => { return SplineMeshBuilder.IsValidResolution((int)value); },
            (object value) => { return SplineMeshBuilder.MINIMUM_RESOLUTION; });
    }

    private void DirtyCheck()
    {
        updateSplineMeshBuilders = NavigationObject_Setup_EditorUtilities.CheckForSplineMeshBuilders(ref data, targetComponent, updateSplineMeshBuilders);

        if (isDirty)
        {
            isDirty = NavigationObject_Setup_EditorUtilities.UpdateSplines(targetComponent);
        }

        if (isSplineWidthDirty)
        {
            isSplineWidthDirty = NavigationObject_Setup_EditorUtilities.UpdateSplineWidth(ref data, targetComponent, previousValue_UpperNavigationWidth, previousValue_LowerNavigationWidth, previousValue_UpperInteractionWidth, previousValue_LowerInteractionWidth);
        }
    }
}
