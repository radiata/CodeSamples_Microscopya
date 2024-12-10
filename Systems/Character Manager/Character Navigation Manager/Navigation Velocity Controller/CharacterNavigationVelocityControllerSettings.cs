using UnityEngine;

[System.Serializable]
public class CharacterNavigationVelocityControllerSettings
{
    [SerializeField]private float agentMaxSpeed = 11f;
    [SerializeField]private float agentMinSpeed = 4f;

    [SerializeField]private float agentBrakeSpeedRequirement = 9f;

    [SerializeField]private float requiredDistanceForMaxSpeed = 10f;

    [SerializeField]private float defaultStoppingDistance = 0f;
    [SerializeField]private float brakeStartDistance = 5f;
    [SerializeField]private float brakeTime = .75f;

    [SerializeField] private float testVelocity = 0f;

    public static string AgentMaxSpeedVariableName = nameof(agentMaxSpeed);
    public static string AgentMinSpeedVariableName = nameof(agentMinSpeed);
    public static string AgentBrakeSpeedRequirementVariableName = nameof(agentBrakeSpeedRequirement);
    public static string RequiredDistanceForMaxSpeedVariableName = nameof(requiredDistanceForMaxSpeed);
    public static string DefaultStoppingDistanceVariableName = nameof(defaultStoppingDistance);
    public static string BrakeStartDistanceVariableName = nameof(brakeStartDistance);
    public static string BrakeTimeVariableName = nameof(brakeTime);
    public static string TestVelocityVariableName = nameof(testVelocity);

    public void ApplyCharacterNavigationVelocityControllerSettings(CharacterNavigationVelocityController characterNavigationVelocityController)
    {
        characterNavigationVelocityController.InitializeVariables(agentMaxSpeed, agentMinSpeed, agentBrakeSpeedRequirement, 
            requiredDistanceForMaxSpeed, defaultStoppingDistance, brakeStartDistance, brakeTime, testVelocity);
    }
}
