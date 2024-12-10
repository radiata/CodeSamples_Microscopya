/// Script source: https://discussions.unity.com/t/generate-2d-polygon-collider-from-3d-mesh/205520
/// By: https://discussions.unity.com/u/alpineboarder

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

public static class Polygon2DMatchToMeshColliderEdges
{
    [MenuItem("Tools/Other/Polygon Collider 2D to Mesh Collider Edges", false, 1000)]
    static void UpdatePolygonColliders()
    {
        Transform transform = Selection.activeTransform;
        if (transform == null)
        {
            Debug.LogWarning("No valid GameObject selected!");
            return;
        }

        EditorSceneManager.MarkSceneDirty(transform.gameObject.scene);

        MeshFilter[] meshFilters = transform.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter meshFilter in meshFilters)
        {
            if (meshFilter.GetComponent<PolygonCollider2D>() != null)
            {
                UpdatePolygonCollider2D(meshFilter);
            }
        }
    }

    static void UpdatePolygonCollider2D(MeshFilter meshFilter)
    {
        if (meshFilter.sharedMesh == null)
        {
            Debug.LogWarning(meshFilter.gameObject.name + " has no Mesh set on its MeshFilter component!");
            return;
        }

        PolygonCollider2D polygonCollider2D = meshFilter.GetComponent<PolygonCollider2D>();
        polygonCollider2D.pathCount = 1;

        List<Vector3> vertices = new List<Vector3>();
        meshFilter.sharedMesh.GetVertices(vertices);

        var boundaryPath = EdgeHelpers.GetEdges(meshFilter.sharedMesh.triangles).FindBoundary().SortEdges();

        Vector3[] yourVectors = new Vector3[boundaryPath.Count];
        for (int i = 0; i < boundaryPath.Count; i++)
        {
            yourVectors[i] = vertices[boundaryPath[i].v1];
        }
        List<Vector2> newColliderVertices = new List<Vector2>();

        for (int i = 0; i < yourVectors.Length; i++)
        {
            newColliderVertices.Add(new Vector2(yourVectors[i].x, yourVectors[i].y));
        }

        Vector2[] newPoints = newColliderVertices.Distinct().ToArray();

        EditorUtility.SetDirty(polygonCollider2D);

        polygonCollider2D.SetPath(0, newPoints);
        Debug.Log(meshFilter.gameObject.name + " PolygonCollider2D updated.");
    }
}