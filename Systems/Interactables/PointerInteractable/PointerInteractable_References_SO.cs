using UnityEngine;

[CreateAssetMenu(fileName = "Pointer Interactable Settings", menuName = "Input Manager/Input Manager Scriptable Objects/Pointer Interactable Settings")]
public class PointerInteractable_References_SO : ScriptableObject
{
    [SerializeField] private LayerMask pointerDetectionLayers;
    [SerializeField] private LayerMask characterNavigationLayers;
    [SerializeField] private LayerMask researchMode_PointerDetectionLayers;

    public LayerMask GetPointerDetectionLayers() => pointerDetectionLayers;
    public LayerMask GetCharacterNavigationLayers() => characterNavigationLayers;
    public LayerMask GetResearchMode_PointerDetectionLayers() => researchMode_PointerDetectionLayers;
}
