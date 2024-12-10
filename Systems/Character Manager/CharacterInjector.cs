using UnityEngine;
using UnityEngine.AI;

public class CharacterInjector : MonoBehaviour
{
    [SerializeField] private CharacterSettings_SO characterSettings;
    [SerializeField] private GameObject characterGameObject;

    private void Awake()
    {
        NavMeshAgent navMeshAgent = characterGameObject.GetComponentInChildren<NavMeshAgent>();
        CharacterNavigationManager characterNavigationManager = characterGameObject.GetComponentInChildren<CharacterNavigationManager>();
        CharacterNavigationVelocityController characterNavigationVelocityController = characterGameObject.GetComponentInChildren<CharacterNavigationVelocityController>();
        CharacterRotationController characterRotationController = characterGameObject.GetComponentInChildren<CharacterRotationController>();
        CharacterFacingReporter characterFacingReporter = characterGameObject.GetComponentInChildren<CharacterFacingReporter>();
        CharacterCameraManager characterCameraManager = characterGameObject.GetComponentInChildren<CharacterCameraManager>();

        characterSettings.ApplyAllSettings(navMeshAgent, characterNavigationManager, characterNavigationVelocityController, characterRotationController, characterFacingReporter, characterCameraManager);
    }
}
