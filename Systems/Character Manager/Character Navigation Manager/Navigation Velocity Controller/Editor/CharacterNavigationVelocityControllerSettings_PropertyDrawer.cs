using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CharacterNavigationVelocityControllerSettings))]
public class CharacterNavigationVelocityControllerSettings_PropertyDrawer : PropertyDrawer
{
    public static bool isExpanded = false;

    private SerializedProperty agentMaxSpeed;
    private GUIContent agentMaxSpeed_Label = new GUIContent("Max Speed for Nav Mesh Agent");

    private SerializedProperty agentMinSpeed;
    private GUIContent agentMinSpeed_Label = new GUIContent("Min Speed for Nav Mesh Agent");

    private SerializedProperty agentBrakeSpeedRequirement;
    private GUIContent agentBrakeSpeedRequirement_Label = new GUIContent("Speed required for Nav Mesh Agent to execute a brake");

    private SerializedProperty requiredDistanceForMaxSpeed;
    private GUIContent requiredDistanceForMaxSpeed_Label = new GUIContent("Distance required for Nav Mesh Agent to use Max Speed");

    private SerializedProperty defaultStoppingDistance;
    private GUIContent defaultStoppingDistance_Label = new GUIContent("Default Stopping Distance for Nav Mash Agent");

    private SerializedProperty brakeStartDistance;
    private GUIContent brakeStartDistance_Label = new GUIContent("Distance from Destination a Brake will Begin");

    private SerializedProperty brakeTime;
    private GUIContent brakeTime_Label = new GUIContent("Time required for a brake to be completed");

    private SerializedProperty testVelocity;
    private GUIContent testVelocity_Label = new GUIContent("Test Velocity (I don't know what this affects yet)");

    private bool isInitialized = false;

    private void Initialize(SerializedProperty property)
    {
        agentMaxSpeed = property.FindPropertyRelative(CharacterNavigationVelocityControllerSettings.AgentMaxSpeedVariableName);
        agentMinSpeed = property.FindPropertyRelative(CharacterNavigationVelocityControllerSettings.AgentMinSpeedVariableName);
        agentBrakeSpeedRequirement = property.FindPropertyRelative(CharacterNavigationVelocityControllerSettings.AgentBrakeSpeedRequirementVariableName);
        requiredDistanceForMaxSpeed = property.FindPropertyRelative(CharacterNavigationVelocityControllerSettings.RequiredDistanceForMaxSpeedVariableName);
        defaultStoppingDistance = property.FindPropertyRelative(CharacterNavigationVelocityControllerSettings.DefaultStoppingDistanceVariableName);
        brakeStartDistance = property.FindPropertyRelative(CharacterNavigationVelocityControllerSettings.BrakeStartDistanceVariableName);
        brakeTime = property.FindPropertyRelative(CharacterNavigationVelocityControllerSettings.BrakeTimeVariableName);
        testVelocity = property.FindPropertyRelative(CharacterNavigationVelocityControllerSettings.TestVelocityVariableName);


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

        EditorGUILayout.LabelField("Speed Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(agentMaxSpeed, agentMaxSpeed_Label);
        EditorGUILayout.PropertyField(agentMinSpeed, agentMinSpeed_Label);

        EditorGUILayout.LabelField("Brake/Slide Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(agentBrakeSpeedRequirement, agentBrakeSpeedRequirement_Label);
        EditorGUILayout.PropertyField(requiredDistanceForMaxSpeed, requiredDistanceForMaxSpeed_Label);
        EditorGUILayout.PropertyField(brakeStartDistance, brakeStartDistance_Label);
        EditorGUILayout.PropertyField(brakeTime, brakeTime_Label);


        EditorGUILayout.LabelField("Other Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(defaultStoppingDistance, defaultStoppingDistance_Label);
        EditorGUILayout.PropertyField(testVelocity, testVelocity_Label);

        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);
        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }

    private bool DrawCollapsibleTitle()
    {
        isExpanded = EditorGUILayout.Foldout(isExpanded, "Character Navigation Velocity Controller Settings", EditorStyles.foldoutHeader);
        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        return isExpanded;
    }
}
