using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NavMeshAgentSettings))]
public class NavMeshAgentSettings_PropertyDrawer : PropertyDrawer
{
    public static bool isExpanded = false;

    private SerializedProperty agentType;
    private GUIContent agentType_Label = new GUIContent("Agent Type");

    private SerializedProperty baseOffset;
    private GUIContent baseOffset_Label = new GUIContent("Base Offset");

    private SerializedProperty speed;
    private GUIContent speed_Label = new GUIContent("Speed");

    private SerializedProperty angularSpeed;
    private GUIContent angularSpeed_Label = new GUIContent("Angular Speed");

    private SerializedProperty acceleration;
    private GUIContent acceleration_Label = new GUIContent("Acceleration");

    private SerializedProperty stoppingDistance;
    private GUIContent stoppingDistance_Label = new GUIContent("Stopping Distance");

    private SerializedProperty autoBraking;
    private GUIContent autoBraking_Label = new GUIContent("Auto Braking");

    private SerializedProperty radius;
    private GUIContent radius_Label = new GUIContent("Radius");

    private SerializedProperty height;
    private GUIContent height_Label = new GUIContent("Height");

    private SerializedProperty quality;
    private GUIContent quality_Label = new GUIContent("Quality");

    private SerializedProperty priority;
    private GUIContent priority_Label = new GUIContent("Priority");

    private SerializedProperty autoTraverseOffMeshLink;
    private GUIContent autoTraverseOffMeshLink_Label = new GUIContent("Auto Traverse Off Mesh Link");

    private SerializedProperty autoRepath;
    private GUIContent autoRepath_Label = new GUIContent("Auto Repath");

    private SerializedProperty areaMask;
    private GUIContent areaMask_Label = new GUIContent("Area Mask");

    private SerializedProperty updateRotation;
    private GUIContent updateRotation_Label = new GUIContent("Update Rotation");

    private bool isInitialized = false;

    private void Initialize(SerializedProperty property)
    {
        agentType = property.FindPropertyRelative(NavMeshAgentSettings.AgentTypeVariableName);
        baseOffset = property.FindPropertyRelative(NavMeshAgentSettings.BaseOffsetVariableName);
        speed = property.FindPropertyRelative(NavMeshAgentSettings.SpeedVariableName);
        angularSpeed = property.FindPropertyRelative(NavMeshAgentSettings.AngularSpeedVariableName);
        acceleration = property.FindPropertyRelative(NavMeshAgentSettings.AccelerationVariableName);
        stoppingDistance = property.FindPropertyRelative(NavMeshAgentSettings.StoppingDistanceVariableName);
        autoBraking = property.FindPropertyRelative(NavMeshAgentSettings.AutoBrakingVariableName);
        radius = property.FindPropertyRelative(NavMeshAgentSettings.RadiusVariableName);
        height = property.FindPropertyRelative(NavMeshAgentSettings.HeightVariableName);
        quality = property.FindPropertyRelative(NavMeshAgentSettings.QualityVariableName);
        priority = property.FindPropertyRelative(NavMeshAgentSettings.PriorityVariableName);
        autoTraverseOffMeshLink = property.FindPropertyRelative(NavMeshAgentSettings.AutoTraverseOffMeshLinkVariableName);
        autoRepath = property.FindPropertyRelative(NavMeshAgentSettings.AutoRepathVariableName);
        areaMask = property.FindPropertyRelative(NavMeshAgentSettings.AreaMaskVariableName);
        updateRotation = property.FindPropertyRelative(NavMeshAgentSettings.UpdateRotationVariableName);

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

        DrawHeaderSection();
        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        DrawSteeringSection();
        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        DrawObstacleAvoidanceSection();
        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        DrawPathFindingSection();
        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);

        DrawHiddenSection();
        EditorGUILayout.Space(CustomEditorUtilities.sectionSpacer);
        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }

    private bool DrawCollapsibleTitle()
    {
        isExpanded = EditorGUILayout.Foldout(isExpanded, "Nav Mesh Agent Settings", EditorStyles.foldoutHeader);
        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        return isExpanded;
    }

    private void DrawHeaderSection()
    {
        EditorGUILayout.PropertyField(agentType, agentType_Label);
        EditorGUILayout.PropertyField(baseOffset, baseOffset_Label);
    }

    private void DrawSteeringSection()
    {
        EditorGUILayout.LabelField("Steering", EditorStyles.boldLabel);
        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(speed, speed_Label);
        EditorGUILayout.PropertyField(angularSpeed, angularSpeed_Label);
        EditorGUILayout.PropertyField(acceleration, acceleration_Label);
        EditorGUILayout.PropertyField(stoppingDistance, stoppingDistance_Label);
        EditorGUILayout.PropertyField(autoBraking, autoBraking_Label);
        EditorGUI.indentLevel--;
    }

    private void DrawObstacleAvoidanceSection()
    {
        EditorGUILayout.LabelField("Obstacle Avoidance", EditorStyles.boldLabel);
        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(radius, radius_Label);
        EditorGUILayout.PropertyField(height, height_Label);
        EditorGUILayout.PropertyField(quality, quality_Label);
        EditorGUILayout.PropertyField(priority, priority_Label);
        EditorGUI.indentLevel--;
    }

    private void DrawPathFindingSection()
    {
        EditorGUILayout.LabelField("Path Finding", EditorStyles.boldLabel);
        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(autoTraverseOffMeshLink, autoTraverseOffMeshLink_Label);
        EditorGUILayout.PropertyField(autoRepath, autoRepath_Label);
        EditorGUILayout.PropertyField(areaMask, areaMask_Label);
        EditorGUI.indentLevel--;
    }

    private void DrawHiddenSection()
    {
        EditorGUILayout.LabelField("Hidden Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space(CustomEditorUtilities.lineSpacer);

        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(updateRotation, updateRotation_Label);
        EditorGUI.indentLevel--;
    }
}
