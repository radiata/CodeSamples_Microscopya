using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CharacterNavigationManagerSettings))]
public class CharacterNavigationManagerSettings_PropertyDrawer : PropertyDrawer
{
    public static bool isExpanded = false;

    private SerializedProperty pathRestrictedByMaxDistance;
    private GUIContent pathRestrictedByMaxDistance_Label = new GUIContent("Restrict Path By Distance");

    private SerializedProperty maxPathingDistance;
    private GUIContent maxPathingDistance_Label = new GUIContent("Max Distance");

    private SerializedProperty pathRestrictedByCameraView;
    private GUIContent pathRestrictedByCameraView_Label = new GUIContent("Restrict Path By Camera View");

    private bool isInitialized = false;

    private void Initialize(SerializedProperty property)
    {
        pathRestrictedByMaxDistance = property.FindPropertyRelative(CharacterNavigationManagerSettings.PathRestrictedByMaxDistanceVariableName);
        maxPathingDistance = property.FindPropertyRelative(CharacterNavigationManagerSettings.MaxPathingDistanceVariableName);
        pathRestrictedByCameraView = property.FindPropertyRelative(CharacterNavigationManagerSettings.PathRestrictedByCameraViewVariableName);

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

        EditorGUILayout.PropertyField(pathRestrictedByMaxDistance, pathRestrictedByMaxDistance_Label);
        EditorGUILayout.PropertyField(maxPathingDistance, maxPathingDistance_Label);
        EditorGUILayout.PropertyField(pathRestrictedByCameraView, pathRestrictedByCameraView_Label);

        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);
        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }

    private bool DrawCollapsibleTitle()
    {
        isExpanded = EditorGUILayout.Foldout(isExpanded, "Character Navigation Manager Settings", EditorStyles.foldoutHeader);
        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        return isExpanded;
    }
}
