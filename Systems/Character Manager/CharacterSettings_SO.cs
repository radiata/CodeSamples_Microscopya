using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Character Settings", menuName = "Character Settings/Character Settings Scriptable Objects/Character Settings")]
public class CharacterSettings_SO : ScriptableObject
{
    [SerializeField] private NavMeshAgentSettings navMeshAgentSettings;
    [SerializeField] private CharacterNavigationManagerSettings characterNavigationManagerSettings;
    [SerializeField] private CharacterNavigationVelocityControllerSettings characterNavigationVelocityControllerSettings;
    [SerializeField] private CharacterRotationControllerSettings characterRotationControllerSettings;
    [SerializeField] private CharacterCameraManagerSettings characterCameraManagerSettings;

    public void ApplyAllSettings(NavMeshAgent navMeshAgent, CharacterNavigationManager characterNavigationManager, CharacterNavigationVelocityController characterNavigationVelocityController,
        CharacterRotationController characterRotationController, CharacterFacingReporter characterFacingReporter, CharacterCameraManager characterCameraManager)
    {
        ApplyNavMeshAgentSettings(navMeshAgent);
        ApplyCharacterNavigationManagerSettings(characterNavigationManager);
        ApplyCharacterNavigationVelocityControllerSettings(characterNavigationVelocityController);
        ApplyCharacterRotationControllerSettings(characterRotationController, characterFacingReporter);
        ApplyCharacterCameraManagerSettings(characterCameraManager);
    }

    private void ApplyCharacterNavigationVelocityControllerSettings(CharacterNavigationVelocityController characterNavigationVelocityController)
    {
        characterNavigationVelocityControllerSettings.ApplyCharacterNavigationVelocityControllerSettings(characterNavigationVelocityController);
    }

    private void ApplyNavMeshAgentSettings(NavMeshAgent navMeshAgent)
    {
        navMeshAgentSettings.ApplyAgentSettings(navMeshAgent);
    }
    private void ApplyCharacterNavigationManagerSettings(CharacterNavigationManager characterNavigationManager)
    {
        characterNavigationManagerSettings.ApplyCharacterNavigationManagerSettings(characterNavigationManager);
    }
    private void ApplyCharacterRotationControllerSettings(CharacterRotationController characterRotationController, CharacterFacingReporter characterFacingReporter)
    {
        characterRotationControllerSettings.ApplyCharacterRotationControllerSettings(characterRotationController, characterFacingReporter);
    }
    private void ApplyCharacterCameraManagerSettings(CharacterCameraManager characterCameraManager)
    {
        characterCameraManagerSettings.ApplyCharacterCameraManagerSettings(characterCameraManager);
    }
}
