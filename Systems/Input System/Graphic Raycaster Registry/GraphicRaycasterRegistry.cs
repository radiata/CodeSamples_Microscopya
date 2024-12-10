using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicRaycasterRegistry : MonoBehaviour
{
    private static List<GraphicRaycaster> graphicRaycasterList;

    public delegate void GraphicRaycasterRegistryUpdated();
    public static event GraphicRaycasterRegistryUpdated OnGraphicRaycasterRegistryUpdate;

    [SerializeField] private GraphicRaycaster graphicRaycaster;

    public static List<GraphicRaycaster> GraphicRaycasterList
    {
        get
        {
            if (graphicRaycasterList == null)
            {
                graphicRaycasterList = new List<GraphicRaycaster>();
            }
            return graphicRaycasterList;
        }
    }

    public void SetGraphicRaycaster(GraphicRaycaster newGraphicRaycaster)
    {
        graphicRaycaster = newGraphicRaycaster;
    }

    public static void RegisterGraphicRaycaster(GraphicRaycaster graphicRaycaster)
    {
        if (graphicRaycasterList == null)
        {
            graphicRaycasterList = new List<GraphicRaycaster>();
        }

        graphicRaycasterList.Add(graphicRaycaster);
    }

    public static void UnregisterGraphicRaycaster(GraphicRaycaster graphicRaycaster)
    {
        graphicRaycasterList.Remove(graphicRaycaster);
    }
    
    public static void UnregisterGraphicRaycaster(GraphicRaycaster[] graphicRaycaster)
    {
        if (graphicRaycasterList == null)
        {
            graphicRaycasterList = new List<GraphicRaycaster>();
        }

        for (int i = 0; i < graphicRaycasterList.Count; i++)
        {
            graphicRaycasterList.Remove(graphicRaycaster[i]);
        }
    }

    private void OnEnable()
    {
        RegisterGraphicRaycaster(graphicRaycaster);
    }

    private void OnDisable()
    {
        UnregisterGraphicRaycaster(graphicRaycaster);
    }

    private void Reset()
    {
        var graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();

        if(graphicRaycaster != null)
        {
            this.graphicRaycaster = graphicRaycaster;
        }
    }
}
