using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CharacterRotationControllerSettings))]
public class CharacterRotationControllerSettings_PropertyDrawer : PropertyDrawer
{
    public static bool isExpanded = false;

    private SerializedProperty rotationSpeed;
    private GUIContent rotationSpeed_Label = new GUIContent("Rotation Speed");

    private SerializedProperty defaultFacingDirection;
    private GUIContent defaultFacingDirection_Label = new GUIContent("Default Facing Rotation");

    private bool isInitialized = false;

    private void Initialize(SerializedProperty property)
    {
        rotationSpeed = property.FindPropertyRelative(CharacterRotationControllerSettings.RotationSpeedVariableName);
        defaultFacingDirection = property.FindPropertyRelative(CharacterRotationControllerSettings.DefaultFacingDirectionVariableName);

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

        EditorGUILayout.PropertyField(rotationSpeed, rotationSpeed_Label);
        EditorGUILayout.PropertyField(defaultFacingDirection, defaultFacingDirection_Label);

        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);
        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }

    private bool DrawCollapsibleTitle()
    {
        isExpanded = EditorGUILayout.Foldout(isExpanded, "Character Rotation Settings", EditorStyles.foldoutHeader);
        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        return isExpanded;
    }
}
