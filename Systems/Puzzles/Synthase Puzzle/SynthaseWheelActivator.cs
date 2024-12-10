using System.Linq;
using UnityEngine;

public class SynthaseWheelActivator : MonoBehaviour, I_ClickResponder
{
    [SerializeField] private PointerInteractable_Base wheelPointerInteractable;
    [SerializeField] private SynthaseWheel synthaseWheel;
    [SerializeField] private Collider2D interactionCollider;
    [SerializeField] private SpriteRenderer wheelActivatorSprite;

    private GameObject synthaseWheel_GameObject;
    private bool wheelActive = false;

    public bool OnClick(Vector3 worldPosition)
    {
        ActivateWheel();
        return true;
    }

    public void EnableInteraction()
    {
        DeactivateWheel();
    }

    public void DisableInteraction()
    {
        DeactivateWheel();
        interactionCollider.gameObject.layer = LayerReferences.NonInteractableLayer;
    }

    private void ActivateWheel()
    {
        InputModule_Character.OnCharacterModuleInput -= UpdateWheelState;
        InputModule_Character.OnCharacterModuleInput += UpdateWheelState;

        wheelActivatorSprite.enabled = false;
        synthaseWheel_GameObject.SetActive(true);
        interactionCollider.gameObject.layer = LayerReferences.NonInteractableLayer;
        wheelActive = true;
    }

    private void DeactivateWheel()
    {
        InputModule_Character.OnCharacterModuleInput -= UpdateWheelState;

        wheelActivatorSprite.enabled = true;
        synthaseWheel_GameObject.SetActive(false);
        interactionCollider.gameObject.layer = LayerReferences.InteractablePuzzleObjectsLayer;
        wheelActive = false;
    }

    private void UpdateWheelState(PointerInteractable_Base[] itemStack)
    {
        if (itemStack.Contains(wheelPointerInteractable))
        {
            return;
        }

        DeactivateWheel();
    }

    private void Awake()
    {
        synthaseWheel_GameObject = synthaseWheel.gameObject;
        synthaseWheel.OnWheelUpdated -= OnWheelUpdated;
        synthaseWheel.OnWheelUpdated += OnWheelUpdated;

    }

    private void OnWheelUpdated()
    {
        if(synthaseWheel.isSolved)
        {
            synthaseWheel.OnWheelUpdated -= OnWheelUpdated;
            DisableInteraction();
        }
    }

    private void OnDestroy()
    {
        synthaseWheel.OnWheelUpdated -= OnWheelUpdated;
        InputModule_Character.OnCharacterModuleInput -= UpdateWheelState;
    }
}
