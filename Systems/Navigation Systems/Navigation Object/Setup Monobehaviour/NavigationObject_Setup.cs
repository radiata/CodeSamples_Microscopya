using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class NavigationObject_Setup : MonoBehaviour
{

    [SerializeField] private SplineContainer pathCoreSpline = null;
    [SerializeField] private Spline cachedCoreSpline = null;

    [SerializeField] private SplineBounds navigationSplineBounds;
    [SerializeField] private SplineBounds interactionSplineBounds;

    [SerializeField] private SplineMeshBuilder navigationSplineMeshBuilder;
    [SerializeField] private int navigationMeshResolution;

    [SerializeField] private SplineMeshBuilder interactionSplineMeshBuilder;
    [SerializeField] private int interactionMeshResolution;

    [SerializeField] private GameObject navigationTemplateGameObject;
    [SerializeField] private GameObject interactionTemplateGameObject;

    #region Getters/Setters
    #region Get Variable names
    public string GetCorePathSplineVariableName() { return nameof(pathCoreSpline); }
    public string GetNavigationSplineBoundsVariableName() { return nameof(navigationSplineBounds); }
    public string GetInteractionSplineBoundsVariableName() { return nameof(interactionSplineBounds); }
    public string GetNavigationSplineMeshBuilderVariableName() { return nameof(navigationSplineMeshBuilder); }
    public string GetNavigationMeshResolutionVariableName() { return nameof(navigationMeshResolution); }
    public string GetInteractionSplineMeshBuilderVariableName() { return nameof(interactionSplineMeshBuilder); }
    public string GetInteractionMeshResolutionVariableName() { return nameof(interactionMeshResolution); }
    public string GetNavigationTemplateGameObjectVariableName() { return nameof(navigationTemplateGameObject); }
    public string GetInteractionTemplateGameObjectVariableName() { return nameof(interactionTemplateGameObject); }


    #endregion
    public SplineContainer GetCorePathSpline()
    {
        return pathCoreSpline;
    }

    public void SetCachedSpline(Spline newSpline)
    {
        if (newSpline == null)
        {
            cachedCoreSpline = null;
            return;
        }

        ForceCorePathToBezier();
        cachedCoreSpline = new Spline();
        cachedCoreSpline.Copy(newSpline);
    }
    public Spline GetCachedSpline()
    {
        return cachedCoreSpline;
    }

    public SplineMeshBuilder GetNavigationSplineMeshBuilder()
    {
        return navigationSplineMeshBuilder;
    }

    public SplineMeshBuilder GetInteractionSplineMeshBuilder()
    {
        return interactionSplineMeshBuilder;
    }
    
    public GameObject GetNavigationTemplateGameObject()
    {
        return navigationTemplateGameObject;
    }
    public GameObject GetInteractionTemplateGameObject()
    {
        return interactionTemplateGameObject;
    }

    public SplineContainer GetNavigationSplineUpperBounds() => navigationSplineBounds.GetUpperBoundsSpline();
    public SplineContainer GetNavigationSplineLowerBounds() => navigationSplineBounds.GetLowerBoundsSpline();

    public SplineContainer GetInteractionSplineUpperBounds() => interactionSplineBounds.GetUpperBoundsSpline();
    public SplineContainer GetInteractionSplineLowerBounds() => interactionSplineBounds.GetLowerBoundsSpline();
    
    public float GetNavigationUpperWidth() => navigationSplineBounds.GetUpperBoundsWidth();
    public float GetNavigationLowerWidth() => navigationSplineBounds.GetLowerBoundsWidth();

    public float GetInteractionUpperWidth() => interactionSplineBounds.GetUpperBoundsWidth();
    public float GetInteractionLowerWidth() => interactionSplineBounds.GetLowerBoundsWidth();

    public int GetNavigationMeshResolution() => navigationMeshResolution;
    public int GetInteractionMeshResolution() => interactionMeshResolution;
    #endregion

    public SplineContainer NewSplinePath(string gameObjectName)
    {
        GameObject newSplineContainer = new GameObject(gameObjectName);
        newSplineContainer.transform.parent = this.transform;

        newSplineContainer.transform.localScale = Vector3.one;
        newSplineContainer.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        return newSplineContainer.AddComponent<SplineContainer>();
    }

    public SplineContainer NewSplinePathFromCorePath(string gameObjectName)
    {
        ForceCorePathToBezier();

        GameObject newSplineContainer = new GameObject(gameObjectName);

        newSplineContainer.transform.parent = pathCoreSpline.transform.parent;
        newSplineContainer.transform.localScale = pathCoreSpline.transform.localScale;
        newSplineContainer.transform.SetLocalPositionAndRotation(pathCoreSpline.transform.localPosition, pathCoreSpline.transform.localRotation);

        var splineContainerComponent = newSplineContainer.AddComponent<SplineContainer>();
        splineContainerComponent.Spline.Copy(pathCoreSpline.Spline);

        return splineContainerComponent;
    }

    public void MoveSplineRelativeToCorePath(SplineContainer splineContainer, Vector3 translationAmount)
    {
        if (splineContainer == null || pathCoreSpline == null || translationAmount == Vector3.zero)
        {
            return;
        }

        ForceCorePathToBezier();

        for (int i = 0; i < pathCoreSpline.Spline.Count; i++)
        {
            BezierKnot knot = splineContainer.Spline[i];

            var translateX = (Quaternion)knot.Rotation * Vector3.right * translationAmount.x;
            var translateY = (Quaternion)knot.Rotation * Vector3.up * translationAmount.y;
            var translateZ = (Quaternion)knot.Rotation * Vector3.forward * translationAmount.z;

            splineContainer.Spline[i] += translateX + translateY + translateZ;
        }
    }

    public void UpdateFromCore(SplineContainer splineContainer)
    {
        if (splineContainer == null)
        {
            return;
        }

        ForceCorePathToBezier();

        if (cachedCoreSpline == null || cachedCoreSpline.Count == 0)
        {
            SetCachedSpline(pathCoreSpline.Spline);
            return;
        }

        //add new knots
        //...

        for (int i = 0; i < pathCoreSpline.Spline.Count; i++)
        {
            //position
            float3 newPosition = splineContainer.Spline[i].Position + (pathCoreSpline.Spline[i].Position - cachedCoreSpline[i].Position);

            //rotation
            Quaternion newCoreQuaternion = pathCoreSpline.Spline[i].Rotation;
            Quaternion cachedCoreQuaternion = cachedCoreSpline[i].Rotation;
            Vector3 eDiff = newCoreQuaternion.eulerAngles - cachedCoreQuaternion.eulerAngles;

            Quaternion newRotation = splineContainer.Spline[i].Rotation;
            newRotation.eulerAngles = newRotation.eulerAngles + eDiff;

            //tangent
            float3 newTangentIn = splineContainer.Spline[i].TangentIn + (pathCoreSpline.Spline[i].TangentIn - cachedCoreSpline[i].TangentIn);
            float3 newTangentOut = splineContainer.Spline[i].TangentOut + (pathCoreSpline.Spline[i].TangentOut - cachedCoreSpline[i].TangentOut);

            //new knot
            splineContainer.Spline[i] = new BezierKnot(newPosition, newTangentIn, newTangentOut, newRotation);
        }

        //modes
        //...
    }

    public void ForceCorePathToBezier()
    {
        pathCoreSpline.Spline.SetTangentMode(TangentMode.Continuous);
    }
}
