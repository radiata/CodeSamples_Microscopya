using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CharacterCameraManagerSettings))]
public class CharacterCameraManagerSettings_PropertyDrawer : PropertyDrawer
{
    public static bool isExpanded = false;

    private SerializedProperty zoomSmoothingSpeed;
    private GUIContent zoomSmoothingSpeed_Label = new GUIContent("Zoom Smoothing for Character Camera");

    private SerializedProperty defaultZoomValue;
    private GUIContent defaultZoomValue_Label = new GUIContent("Zoom Default Value for Character Camera");

    private SerializedProperty minimumZoomValue;
    private GUIContent minimumZoomValue_Label = new GUIContent("Minimum Zoom Value for Character Camera");

    private SerializedProperty maximumZoomValue;
    private GUIContent maximumZoomValue_Label = new GUIContent("Maximum Zoom Value for Character Camera");

    private SerializedProperty tiltSmoothingSpeed;
    private GUIContent tiltSmoothingSpeed_Label = new GUIContent("Tilt Smoothing for Character Camera");

    private SerializedProperty tiltWeight;
    private GUIContent tiltWeight_Label = new GUIContent("Tilt Weight Value for Character Camera");

    private bool isInitialized = false;

    private void Initialize(SerializedProperty property)
    {
        zoomSmoothingSpeed = property.FindPropertyRelative(CharacterCameraManagerSettings.ZoomSmoothingSpeedVariableName);
        defaultZoomValue = property.FindPropertyRelative(CharacterCameraManagerSettings.DefaultZoomValueVariableName);
        minimumZoomValue = property.FindPropertyRelative(CharacterCameraManagerSettings.MinimumZoomValueVariableName);
        maximumZoomValue = property.FindPropertyRelative(CharacterCameraManagerSettings.MaximumZoomValueVariableName);
        tiltSmoothingSpeed = property.FindPropertyRelative(CharacterCameraManagerSettings.TiltSmoothingSpeedVariableName);
        tiltWeight = property.FindPropertyRelative(CharacterCameraManagerSettings.TiltWeightVariableName);

        isInitialized = true;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (isInitialized == false)
        {
            Initialize(property);
        }

        EditorGUI.BeginProperty(position, label, property);

        if (DrawCollapsibleTitle() == false)
        {
            EditorGUI.EndProperty();
            return;
        }

        EditorGUI.indentLevel++;

        EditorGUILayout.LabelField("Zoom Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(zoomSmoothingSpeed, zoomSmoothingSpeed_Label);
        EditorGUILayout.PropertyField(defaultZoomValue, defaultZoomValue_Label);
        EditorGUILayout.PropertyField(minimumZoomValue, minimumZoomValue_Label);
        EditorGUILayout.PropertyField(maximumZoomValue, maximumZoomValue_Label);
        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        EditorGUILayout.LabelField("Tilt Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(tiltSmoothingSpeed, tiltSmoothingSpeed_Label);
        EditorGUILayout.PropertyField(tiltWeight, tiltWeight_Label);

        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);
        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }

    private bool DrawCollapsibleTitle()
    {
        isExpanded = EditorGUILayout.Foldout(isExpanded, "Character Camera Manager Settings", EditorStyles.foldoutHeader);
        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        return isExpanded;
    }
}
