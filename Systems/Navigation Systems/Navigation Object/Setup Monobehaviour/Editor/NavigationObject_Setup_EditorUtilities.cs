using Unity.AI.Navigation;
using Unity.AI.Navigation.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

public static class NavigationObject_Setup_EditorUtilities
{
    internal static void AssignProperties(ref NavigationObject_Setup_EditorData data, SerializedObject serializedObject, NavigationObject_Setup targetComponent)
    {
        data.corePathSpline = serializedObject.FindProperty(targetComponent.GetCorePathSplineVariableName());

        data.navigationSplineBounds = serializedObject.FindProperty(targetComponent.GetNavigationSplineBoundsVariableName());
        data.upperNavigationSpline = data.navigationSplineBounds.FindPropertyRelative(SplineBounds.GetUpperBoundsSplineVariableName());
        data.lowerNavigationSpline = data.navigationSplineBounds.FindPropertyRelative(SplineBounds.GetLowerBoundsSplineVariableName());
        data.upperNavigationWidth = data.navigationSplineBounds.FindPropertyRelative(SplineBounds.GetUpperBoundsWidthVariableName());
        data.lowerNavigationWidth = data.navigationSplineBounds.FindPropertyRelative(SplineBounds.GetLowerBoundsWidthVariableName());

        data.interactionSplineBounds = serializedObject.FindProperty(targetComponent.GetInteractionSplineBoundsVariableName());
        data.upperInteractionSpline = data.interactionSplineBounds.FindPropertyRelative(SplineBounds.GetUpperBoundsSplineVariableName());
        data.lowerInteractionSpline = data.interactionSplineBounds.FindPropertyRelative(SplineBounds.GetLowerBoundsSplineVariableName());
        data.upperInteractionWidth = data.interactionSplineBounds.FindPropertyRelative(SplineBounds.GetUpperBoundsWidthVariableName());
        data.lowerInteractionWidth = data.interactionSplineBounds.FindPropertyRelative(SplineBounds.GetLowerBoundsWidthVariableName());

        data.navigationSplineMeshBuilder = serializedObject.FindProperty(targetComponent.GetNavigationSplineMeshBuilderVariableName());
        data.navigationMeshResolution = serializedObject.FindProperty(targetComponent.GetNavigationMeshResolutionVariableName());

        data.interactionSplineMeshBuilder = serializedObject.FindProperty(targetComponent.GetInteractionSplineMeshBuilderVariableName());
        data.interactionMeshResolution = serializedObject.FindProperty(targetComponent.GetInteractionMeshResolutionVariableName());
        
        data.navigationTemplateGameObject = serializedObject.FindProperty(targetComponent.GetNavigationTemplateGameObjectVariableName());
        data.interactionTemplateGameObject = serializedObject.FindProperty(targetComponent.GetInteractionTemplateGameObjectVariableName());
    }

    internal static void GenerateMesh(SplineMeshBuilder splineMeshBuilder, Vector3 localPosition)
    {
        splineMeshBuilder.DestroyExistingGameObject();
        Vector2[] polygon2DVerts = SplineMeshBuilder.GetPolygon2DVerts(splineMeshBuilder.GetMeshResolution(), splineMeshBuilder.GetTopBounds().Spline, splineMeshBuilder.GetBottomBounds().Spline);
        var createdObject = splineMeshBuilder.CreateGameObjectFromTemplate(splineMeshBuilder.GetTemplateGameObject(), polygon2DVerts, localPosition);
        splineMeshBuilder.SetGeneratedGameObject(createdObject);
        BuildNavMeshFromPolygon2D(createdObject, polygon2DVerts);
    }

    internal static void BuildNavMeshFromPolygon2D(GameObject rootObject, Vector2[] polygon2DVerts)
    {
        NavMeshSurface navMeshSurface = rootObject.GetComponentInChildren<NavMeshSurface>();
        if (navMeshSurface != null)
        {
            PolygonCollider2D collider = rootObject.GetComponentInChildren<PolygonCollider2D>();
            
            GameObject temporaryGameObject = new GameObject();
            temporaryGameObject.transform.position = Vector3.zero;
            PolygonCollider2D temporaryCollider = temporaryGameObject.AddComponent<PolygonCollider2D>();
            temporaryCollider.points = polygon2DVerts;
            
            Mesh polygonMesh = temporaryCollider.CreateMesh(false, false);

            MeshFilter meshFilter = collider.gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = polygonMesh;
            MeshRenderer meshRenderer = collider.gameObject.AddComponent<MeshRenderer>();

            NavMeshAssetManager.instance.StartBakingSurfaces(new NavMeshSurface[] { navMeshSurface });
            //navMeshSurface.BuildNavMesh();

            GameObject.DestroyImmediate(polygonMesh);
            GameObject.DestroyImmediate(temporaryGameObject);
            GameObject.DestroyImmediate(meshFilter);
            GameObject.DestroyImmediate(meshRenderer);
        }
    }

    internal static bool GenerateBounds(ref NavigationObject_Setup_EditorData data, NavigationObject_Setup targetComponent, bool generateNavigation, bool generateInteraction)
    {
        bool returnValue = false;

        if(generateNavigation)
        {
            CreateNewSpline(data.upperNavigationSpline, true, NavigationObject_Setup_EditorData.UpperNavigationSplineName, NavigationObject_Setup_EditorData.NewPathSplineFromCore_ButtonText, Vector3.right, data.upperNavigationWidth.floatValue, targetComponent);
            CreateNewSpline(data.lowerNavigationSpline, true, NavigationObject_Setup_EditorData.LowerNavigationSplineName, NavigationObject_Setup_EditorData.NewPathSplineFromCore_ButtonText, Vector3.left, data.lowerNavigationWidth.floatValue, targetComponent);
            returnValue = true;
        }

        if (generateInteraction)
        {
            CreateNewSpline(data.upperInteractionSpline, true, NavigationObject_Setup_EditorData.UpperInteractionSplineName, NavigationObject_Setup_EditorData.NewPathSplineFromCore_ButtonText, Vector3.right, data.upperInteractionWidth.floatValue, targetComponent);
            CreateNewSpline(data.lowerInteractionSpline, true, NavigationObject_Setup_EditorData.LowerInteractionSplineName, NavigationObject_Setup_EditorData.NewPathSplineFromCore_ButtonText, Vector3.left, data.lowerInteractionWidth.floatValue, targetComponent);
            returnValue = true;
        }

        return returnValue;
    }

    internal static SplineContainer CreateNewSpline(SerializedProperty serializedProperty, bool fromCorePath, string newPathName, string undoRedoMessage, Vector3 offsetDirection, float offsetDistance, NavigationObject_Setup targetComponent)
    {
        if (serializedProperty.boxedValue != null)
        {
            GameObject.DestroyImmediate(((SplineContainer)serializedProperty.boxedValue).gameObject);
        }

        serializedProperty.boxedValue = fromCorePath ? targetComponent.NewSplinePathFromCorePath(newPathName) : targetComponent.NewSplinePath(newPathName);
        serializedProperty.serializedObject.ApplyModifiedProperties();

        SplineContainer targetContainer = (SplineContainer)serializedProperty.boxedValue;

        Undo.RegisterCreatedObjectUndo(targetContainer.gameObject, undoRedoMessage);

        var translate = fromCorePath ? offsetDirection.normalized * offsetDistance : Vector3.zero;
        targetComponent.MoveSplineRelativeToCorePath(targetContainer, translate);

        return targetContainer;
    }

    internal static bool CheckForSplineMeshBuilders(ref NavigationObject_Setup_EditorData data, NavigationObject_Setup targetComponent, bool updateSplineMeshBuilders)
    {
        if (data.navigationSplineMeshBuilder.boxedValue == null)
        {
            data.navigationSplineMeshBuilder.boxedValue = targetComponent.gameObject.AddComponent<SplineMeshBuilder>();
            ((SplineMeshBuilder)data.navigationSplineMeshBuilder.boxedValue).SetMeshName(targetComponent.name + " - Navigation Mesh");
            return true;
        }
        if (data.interactionSplineMeshBuilder.boxedValue == null)
        {
            data.interactionSplineMeshBuilder.boxedValue = targetComponent.gameObject.AddComponent<SplineMeshBuilder>();
            ((SplineMeshBuilder)data.interactionSplineMeshBuilder.boxedValue).SetMeshName(targetComponent.name + " - Interaction Mesh");
            return true;
        }

        if (updateSplineMeshBuilders == false)
        {
            return false;
        }

        targetComponent.GetNavigationSplineMeshBuilder().SetCenterPath(targetComponent.GetCorePathSpline());
        targetComponent.GetNavigationSplineMeshBuilder().SetTopBounds(targetComponent.GetNavigationSplineUpperBounds());
        targetComponent.GetNavigationSplineMeshBuilder().SetBottomBounds(targetComponent.GetNavigationSplineLowerBounds());
        targetComponent.GetNavigationSplineMeshBuilder().SetTemplateGameObject(targetComponent.GetNavigationTemplateGameObject());        
        targetComponent.GetNavigationSplineMeshBuilder().SetMeshResolution(data.navigationMeshResolution.intValue);

        targetComponent.GetInteractionSplineMeshBuilder().SetTopBounds(targetComponent.GetInteractionSplineUpperBounds());
        targetComponent.GetInteractionSplineMeshBuilder().SetBottomBounds(targetComponent.GetInteractionSplineLowerBounds());
        targetComponent.GetInteractionSplineMeshBuilder().SetTemplateGameObject(targetComponent.GetInteractionTemplateGameObject());        
        targetComponent.GetInteractionSplineMeshBuilder().SetMeshResolution(data.interactionMeshResolution.intValue);

        SplineContainer validCorePath = targetComponent.GetCorePathSpline();
        if (validCorePath != null)
        {
            targetComponent.GetNavigationSplineMeshBuilder().SetTargetPlacement(validCorePath.gameObject.transform);
            targetComponent.GetInteractionSplineMeshBuilder().SetTargetPlacement(validCorePath.gameObject.transform);
        }

        return false;
    }

    internal static bool UpdateSplines(NavigationObject_Setup targetComponent)
    {
        targetComponent.UpdateFromCore(targetComponent.GetNavigationSplineUpperBounds());
        targetComponent.UpdateFromCore(targetComponent.GetNavigationSplineLowerBounds());
        targetComponent.UpdateFromCore(targetComponent.GetInteractionSplineUpperBounds());
        targetComponent.UpdateFromCore(targetComponent.GetInteractionSplineLowerBounds());

        if (targetComponent.GetCorePathSpline() == null)
        {
            targetComponent.SetCachedSpline(null);
        }
        else
        {
            targetComponent.SetCachedSpline(targetComponent.GetCorePathSpline().Spline);
        }

        return false;
    }

    internal static bool UpdateSplineWidth(ref NavigationObject_Setup_EditorData data, NavigationObject_Setup targetComponent, float previousUpperNavigationWidth, float previousLowerNavigationWidth, float previousUpperInteractionWidth, float previousLowerInteractionWidth)
    {
        if (data.upperNavigationWidth.floatValue - previousUpperNavigationWidth != 0)
        {
            Vector3 translate = Vector3.right * (data.upperNavigationWidth.floatValue - previousUpperNavigationWidth);
            targetComponent.MoveSplineRelativeToCorePath((SplineContainer)data.upperNavigationSpline.boxedValue, translate);
        }

        if (data.lowerNavigationWidth.floatValue - previousLowerNavigationWidth != 0)
        {
            Vector3 translate = Vector3.left * (data.lowerNavigationWidth.floatValue - previousLowerNavigationWidth);
            targetComponent.MoveSplineRelativeToCorePath((SplineContainer)data.lowerNavigationSpline.boxedValue, translate);
        }

        if (data.upperInteractionWidth.floatValue - previousUpperInteractionWidth != 0)
        {
            Vector3 translate = Vector3.right * (data.upperInteractionWidth.floatValue - previousUpperInteractionWidth);
            targetComponent.MoveSplineRelativeToCorePath((SplineContainer)data.upperInteractionSpline.boxedValue, translate);
        }

        if (data.lowerInteractionWidth.floatValue - previousLowerInteractionWidth != 0)
        {
            Vector3 translate = Vector3.left * (data.lowerInteractionWidth.floatValue - previousLowerInteractionWidth);
            targetComponent.MoveSplineRelativeToCorePath((SplineContainer)data.lowerInteractionSpline.boxedValue, translate);
        }

        return false;
    }
}
