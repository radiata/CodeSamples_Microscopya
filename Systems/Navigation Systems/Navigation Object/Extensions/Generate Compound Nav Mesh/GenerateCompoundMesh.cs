using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

#if UNITY_EDITOR
using System.Linq;
using Unity.AI.Navigation.Editor;
#endif

public class GenerateCompoundMesh : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private List<NavigationObject_Setup> navigationObjects;

    [SerializeField] private float rotationX = -90;


    #if UNITY_EDITOR
    [ContextMenu("Generate Compound Mesh")]
    private void GenerateNavMesh()
    {
        if (this.transform.rotation != Quaternion.Euler(rotationX, 0, 0))
        {
            Debug.Log(
                $"Changing rotation from {this.transform.rotation} to {Quaternion.Euler(rotationX, 0, 0)} on {this.gameObject}"
                , navMeshSurface.gameObject);

            this.transform.rotation = Quaternion.Euler(rotationX, 0, 0);
        }

        foreach (NavigationObject_Setup navigationObject in navigationObjects)
        {
            Collider2D navMeshCollider = navigationObject.gameObject.GetComponentInChildren<NavigationObject>().NavMeshCollider;

            //type check early to catch errors before creating any new objects
            if(navMeshCollider is PolygonCollider2D == false)
            {
                Debug.LogError($"NavMeshCollider on {navMeshCollider.gameObject} is of unsupported type.", navMeshCollider.gameObject);
                return;
            }

            GameObject temporaryGameObject = new GameObject($"{navigationObject.gameObject.name} - Nav Mesh");
            temporaryGameObject.transform.SetParent(navMeshSurface.transform);
            temporaryGameObject.transform.position = Vector3.zero;

            PolygonCollider2D temporaryCollider = temporaryGameObject.AddComponent<PolygonCollider2D>();
            temporaryCollider.points = ((PolygonCollider2D)navMeshCollider).points;

            Mesh polygonMesh = temporaryCollider.CreateMesh(false, false);

            MeshFilter meshFilter = temporaryGameObject.gameObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = polygonMesh;
            MeshRenderer meshRenderer = temporaryGameObject.gameObject.AddComponent<MeshRenderer>();
        }

        NavMeshAssetManager.instance.StartBakingSurfaces(new NavMeshSurface[] { navMeshSurface });

        List<Transform> objectsToCleanup = navMeshSurface.GetComponentsInChildren<Transform>().ToList();

        objectsToCleanup.Remove(navMeshSurface.transform);
        objectsToCleanup.Remove(this.transform);


        for (int i = objectsToCleanup.Count - 1; i >= 0 ; i--)
        {
            GameObject.DestroyImmediate(objectsToCleanup[i].gameObject.GetComponent<MeshFilter>()?.sharedMesh);
            GameObject.DestroyImmediate(objectsToCleanup[i].gameObject);
        }

        foreach (NavigationObject_Setup navigationObject in navigationObjects)
        {
            NavMeshSurface[] surfaces = navigationObject.GetComponentsInChildren<NavMeshSurface>();
            for (int i = surfaces.Length - 1; i >= 0 ; i--)
            {
                Debug.Log($"Clearing NavMeshSurface on {surfaces[i].gameObject.name}", surfaces[i].gameObject);
            }
            NavMeshAssetManager.instance.ClearSurfaces(surfaces);
        }
    }
    #endif
}
