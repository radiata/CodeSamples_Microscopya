using UnityEngine;

[System.Serializable]
public class CharacterRotationControllerSettings
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private FacingDirection defaultFacingDirection = FacingDirection.left;

    public static string RotationSpeedVariableName => nameof(rotationSpeed);
    public static string DefaultFacingDirectionVariableName => nameof(defaultFacingDirection);

    public void ApplyCharacterRotationControllerSettings(CharacterRotationController characterRotationController, CharacterFacingReporter characterFacingReporter)
    {
        characterRotationController.SetRotationSpeed(rotationSpeed);
        characterFacingReporter.SetDefaultFacingDirection(defaultFacingDirection);
    }
}
