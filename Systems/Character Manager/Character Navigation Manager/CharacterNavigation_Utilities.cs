using UnityEngine;

public class CharacterNavigation_Utilities : MonoBehaviour
{
    [SerializeField] private GameObject characterModel;

    public bool isWorldPositionInCharacterForwardDirection(Vector3 worldPosition)
    {
        Vector3 directionNormalized = (worldPosition - characterModel.transform.position).normalized;

        if (Vector3.Dot(characterModel.transform.right, directionNormalized) > 0)
        {
            return true;
        }
        return false;
    }
}
