using UnityEditor.EditorTools;
using UnityEngine;

namespace UnityEditor.Splines
{
    public class SplineAccess : MonoBehaviour
    {
        public static void SplineToolAccess()
        {
            ActiveEditorTracker.sharedTracker.RebuildIfNecessary();
            //Ensuring trackers are rebuilt before changing to SplineContext
            EditorApplication.delayCall += SetKnotPlacementTool;
        }

        static void SetKnotPlacementTool()
        {
            ToolManager.SetActiveContext<SplineToolContext>();
            ToolManager.SetActiveTool<KnotPlacementTool>();
            EditorApplication.delayCall -= SetKnotPlacementTool;
        }
    }
}
