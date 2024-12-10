using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class ApplyScaleChange_Spline : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Vector3 scale;

    [ContextMenu("Commit Scale Change")]
    private void MultiplySplineKnotsPositionsByScale()
    {
        var list = splineContainer.Spline.ToList();

        for (int i = 0; i < splineContainer.Spline.Count(); i++)
        {
            var knot = list[i];
            knot.Position = new Vector3(knot.Position.x * scale.x, knot.Position.y * scale.y, knot.Position.z * scale.z);
            splineContainer.Spline.SetKnot(i, knot);
        }

        Debug.Log("Scaling complete");
    }
}
