using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SplineBounds))]
public class SplineBounds_PropertyDrawer : PropertyDrawer
{
    private bool isInitialized = false;

    private SerializedProperty upperBoundsSpline;
    private GUIContent upperBoundsSpline_Label = new GUIContent("Upper Bounds Spline");

    private SerializedProperty upperBoundsWidth;
    private GUIContent upperBoundsWidth_Label = new GUIContent("Upper Bounds Width");

    private SerializedProperty lowerBoundsSpline;
    private GUIContent lowerBoundsSpline_Label = new GUIContent("Lower Bounds Spline");

    private SerializedProperty lowerBoundsWidth;
    private GUIContent lowerBoundsWidth_Label = new GUIContent("Lower Bounds Width");


    private void Initialize(SerializedProperty property)
    {
        upperBoundsSpline = property.FindPropertyRelative(SplineBounds.GetUpperBoundsSplineVariableName());
        upperBoundsWidth = property.FindPropertyRelative(SplineBounds.GetUpperBoundsWidthVariableName());

        lowerBoundsSpline = property.FindPropertyRelative(SplineBounds.GetLowerBoundsSplineVariableName());
        lowerBoundsWidth = property.FindPropertyRelative(SplineBounds.GetLowerBoundsWidthVariableName());

        isInitialized = true;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (isInitialized == false)
        {
            Initialize(property);
        }

        EditorGUI.BeginProperty(position, label, property);

        EditorGUILayout.PropertyField(upperBoundsSpline, upperBoundsSpline_Label);

        CustomEditorUtilities.ValidatedProperty(upperBoundsWidth, upperBoundsWidth_Label,
            (object value) => { return SplineBounds.IsValidWidth((float)value); },
            (object value) => { return SplineBounds.MINIMUM_WIDTH; });

        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        EditorGUILayout.PropertyField(lowerBoundsSpline, lowerBoundsSpline_Label);

        CustomEditorUtilities.ValidatedProperty(lowerBoundsWidth, lowerBoundsWidth_Label,
            (object value) => { return SplineBounds.IsValidWidth((float)value); },
            (object value) => { return SplineBounds.MINIMUM_WIDTH; });

        EditorGUI.EndProperty();
    }
}
