using UnityEngine;

[System.Serializable]
public class CharacterNavigationManagerSettings
{
    [SerializeField] private bool pathRestrictedByMaxDistance = false;
    [SerializeField] private float maxPathingDistance;

    [SerializeField] private bool pathRestrictedByCameraView = false;

    public static string PathRestrictedByMaxDistanceVariableName => nameof(pathRestrictedByMaxDistance);
    public static string MaxPathingDistanceVariableName => nameof(maxPathingDistance);
    public static string PathRestrictedByCameraViewVariableName => nameof(pathRestrictedByCameraView);

    public void ApplyCharacterNavigationManagerSettings(CharacterNavigationManager characterNavigationManager)
    {
        characterNavigationManager.SetPathingRestrictions(pathRestrictedByMaxDistance, maxPathingDistance, pathRestrictedByCameraView);
    }
}
