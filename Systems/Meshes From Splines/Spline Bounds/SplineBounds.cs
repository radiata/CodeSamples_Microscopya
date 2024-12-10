using UnityEngine;
using UnityEngine.Splines;

[System.Serializable]
public class SplineBounds
{
    public const float MINIMUM_WIDTH = 0;

    [SerializeField] private SplineContainer upperBoundsSpline = null;
    [SerializeField] private float upperBoundsWidth = 0f;

    [SerializeField] private SplineContainer lowerBoundsSpline = null;
    [SerializeField] private float lowerBoundsWidth = 0f;

    public static string GetUpperBoundsSplineVariableName() { return nameof(upperBoundsSpline); }
    public static string GetUpperBoundsWidthVariableName() { return nameof(upperBoundsWidth); }
    public static string GetLowerBoundsSplineVariableName() { return nameof(lowerBoundsSpline); }
    public static string GetLowerBoundsWidthVariableName() { return nameof(lowerBoundsWidth); }
    public static bool IsValidWidth(float newWidth) => newWidth >= MINIMUM_WIDTH;

    public float GetUpperBoundsWidth() => upperBoundsWidth;
    public float GetLowerBoundsWidth() => lowerBoundsWidth;

    public SplineContainer GetUpperBoundsSpline() => upperBoundsSpline;
    public SplineContainer GetLowerBoundsSpline() => lowerBoundsSpline;

}
