using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

[System.Serializable]
public class SplineMeshBuilder : MonoBehaviour
{
    public const int MINIMUM_RESOLUTION = 1;

    [SerializeField] private string meshName = null;

    [SerializeField] private int meshResolution = 10;

    [SerializeField] private SplineContainer centerPath;
    [SerializeField] private SplineContainer topBounds;
    [SerializeField] private SplineContainer bottomBounds;

    [SerializeField] private GameObject generatedGameObject;
    [SerializeField] private GameObject templateGameObject;
    [SerializeField] private Transform targetPlacement;

    #region Getters/Setters
    #region Get Variable Names
    public static string GetCenterPathVariableName() { return nameof(centerPath); }
    public static string GetTopBoundsVariableName() { return nameof(topBounds); }
    public static string GetBottomBoundsVariableName() { return nameof(bottomBounds); }
    public static string GetMeshResolutionVariableName() { return nameof(meshResolution); }
    public static string GetMeshNameVariableName() { return nameof(meshName); }
    public static string GetTemplateGameObjectVariableName() { return nameof(templateGameObject); }
    public static string GetTargetPlacementVariableName() { return nameof(targetPlacement); }
    #endregion
    public void SetCenterPath(SplineContainer newCenterPath)
    {
        centerPath = newCenterPath;
    }

    public void SetTopBounds(SplineContainer newTopBounds)
    {
        topBounds = newTopBounds;
    }
    public SplineContainer GetTopBounds()
    {
        return topBounds;
    }

    public void SetBottomBounds(SplineContainer newBottomBounds)
    {
        bottomBounds = newBottomBounds;
    }
    public SplineContainer GetBottomBounds()
    {
        return bottomBounds;
    }

    public void SetMeshName(string newMeshName)
    {
        meshName = newMeshName;
    }
    public string GetMeshName()
    {
        return meshName;
    }

    public void SetMeshResolution(int newMeshResolution)
    {
        meshResolution = newMeshResolution;
    }
    public int GetMeshResolution()
    {
        return meshResolution;
    }

    public void SetTemplateGameObject(GameObject newTemplateGameObject)
    {
        templateGameObject = newTemplateGameObject;
    }
    public GameObject GetTemplateGameObject()
    {
        return templateGameObject;
    }

    public void SetTargetPlacement(Transform newTargetPlacement)
    {
        targetPlacement = newTargetPlacement;
    }
    public Transform GetTargetPlacement()
    {
        return targetPlacement;
    }

    public void SetGeneratedGameObject(GameObject newGeneratedGameObject)
    {
        generatedGameObject = newGeneratedGameObject;
    }
    public GameObject GetGeneratedGameObject()
    {
        return generatedGameObject;
    }

    public static bool IsValidResolution(int newResolution) => newResolution >= MINIMUM_RESOLUTION;
    #endregion

    public GameObject CreateGameObjectFromTemplate(GameObject templateObject, Vector2[] polygon2DVerts, Vector3 targetPosition)
    {
        GameObject newGameObject = Instantiate(templateObject, transform);
        newGameObject.transform.localPosition = targetPosition;
        newGameObject.name = meshName;

        AssignComponents.SetupComponents(newGameObject.gameObject, polygon2DVerts, centerPath, topBounds, bottomBounds);

        return newGameObject;
    }

    public void DestroyExistingGameObject()
    {
        DestroyImmediate(generatedGameObject);
    }

    private static Vector3[] VertsFromSpline(int resolution, Spline spline)
    {
        List<Vector3> verts = new List<Vector3>();

        for (int i = 0; i <= resolution; i++)
        {
            float evaluationPoint = (float)i / (float)resolution;
            verts.Add(spline.EvaluatePosition(evaluationPoint));
        }

        return verts.ToArray();
    }

    private int[] TrianglesFromVerts(Vector3[] topVerts, Vector3[] bottomVerts)
    {
        if (topVerts.Length != bottomVerts.Length)
        {
            Debug.LogError("Vert Arrays vary in length. Could not create Triangles.", gameObject);
            return null;
        }

        List<int> triangles = new List<int>();

        for (int i = 0; i < topVerts.Length - 1; i++)
        {
            triangles.Add(i + 1); // start at top left vert
            triangles.Add(i); // draw to right neighbor
            triangles.Add(topVerts.Length + i); // get right neighbors bottom correspondent

            triangles.Add(i + 1); // start at top left vert
            triangles.Add(topVerts.Length + i); //get right neighbors bottom correspondent
            triangles.Add(topVerts.Length + i + 1); //get right neighbors bottom correspondent's left neighbor
        }

        return triangles.ToArray();
    }

    [ContextMenu("Create game objects on verts")]
    private void SplineMarkers(Spline topSpline = null, Spline bottomSpline = null)
    {
        Vector3[] bottomVerts;
        Vector3[] topVerts;

        if (bottomSpline == null)
        {
            bottomVerts = VertsFromSpline(meshResolution, bottomBounds.Spline);
        }
        else
        {
            bottomVerts = VertsFromSpline(meshResolution, bottomSpline);

        }
        if(topSpline == null)
        {
            topVerts = VertsFromSpline(meshResolution, topBounds.Spline);
        }
        else
        {
            topVerts = VertsFromSpline(meshResolution, topSpline);
        }

        var parentGO = new GameObject("Top  Markers");
        parentGO.transform.position = bottomBounds.transform.position;

        foreach (Vector3 point in topVerts)
        {
            var newGO = new GameObject("Marker");
            newGO.transform.SetParent(parentGO.transform);
            newGO.transform.localPosition = point;
            //newGO.transform.position = bottomBounds.transform.InverseTransformPoint(point);
            newGO.AddComponent<MarkerIcon>();
            newGO.transform.localScale = Vector3.one * .1f;
        }

        parentGO = new GameObject("Bottom  Markers");
        parentGO.transform.position = bottomBounds.transform.position;

        foreach (Vector3 point in bottomVerts)
        {
            var newGO = new GameObject("Marker");
            newGO.transform.SetParent(parentGO.transform);
            newGO.transform.localPosition = point;
            //newGO.transform.position = bottomBounds.transform.InverseTransformPoint(point);
            newGO.AddComponent<MarkerIcon>();
            newGO.transform.localScale = Vector3.one * .1f;
        }
    }

    [ContextMenu("Create Polygon Collider 2D")]
    private void GeneratePolygon2D()
    {
        GameObject polygonCollider_GO = new GameObject("Polygon Collider");
        PolygonCollider2D polygonCollider = polygonCollider_GO.AddComponent<PolygonCollider2D>();

        Vector3[] topVerts = VertsFromSpline(meshResolution, topBounds.Spline);
        Vector3[] bottomVerts = VertsFromSpline(meshResolution, bottomBounds.Spline);

        Array.Reverse(bottomVerts);
        Vector3[] mergedVector3Verts = topVerts.Concat(bottomVerts).ToArray();

        Vector2[] Vector2Verts = new Vector2[mergedVector3Verts.Length];
        for (int i = 0; i < mergedVector3Verts.Length; i++)
        {
            Vector2Verts[i] = mergedVector3Verts[i];
        }

        polygonCollider.points = Vector2Verts;
    }

    public static Vector2[] GetPolygon2DVerts(int meshResolution, Spline topBounds, Spline bottomBounds)
    {
        Vector3[] topVerts = VertsFromSpline(meshResolution, topBounds);
        Vector3[] bottomVerts = VertsFromSpline(meshResolution, bottomBounds);

        Array.Reverse(bottomVerts);
        Vector3[] mergedVector3Verts = topVerts.Concat(bottomVerts).ToArray();

        Vector2[] Vector2Verts = new Vector2[mergedVector3Verts.Length];
        for (int i = 0; i < mergedVector3Verts.Length; i++)
        {
            Vector2Verts[i] = mergedVector3Verts[i];
        }

        return Vector2Verts;
    }
}
