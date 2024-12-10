using UnityEngine;

public class TriggerNavigation_PointerInteractable : PointerInteractable_Base
{
    [SerializeField] private Transform targetNavigationPosition;

    [SerializeField] private GameObject navigationObject_RootObject;
    [SerializeField, HideInInspector] private NavigationObject interactionPositionNavigationObject;

    public override int PriorityValue() => PointerInteractable_References.NavigationTrigger;

    public override bool IsClickable() => true;
    public override bool IsDraggable() => false;
    public override bool IsHoldable() => true;
    public override bool IsSwipeable() => true;
    public override bool IsPointerContactStartable() => false;

    public override bool SendPassThrough() => false;
    public override bool ReceivePassThrough() => false;

    public override PointerInteractable_Base HoldStart(Vector3 worldPosition, out bool consumed)
    {
        interactionPositionNavigationObject.Navigate(targetNavigationPosition.position);
        consumed = true;
        return this;
    }

    public override void Holding(Vector3 worldPosition, Vector3 cameraForward)
    {
        return;
    }

    public override void HoldEnd(Vector3 worldPosition)
    {
        interactionPositionNavigationObject.Navigate(targetNavigationPosition.position);
    }

    public override bool Click(Vector3 worldPosition, Vector3 cameraForward)
    {
        interactionPositionNavigationObject.Navigate(targetNavigationPosition.position);
        return true;
    }

    public override void Drag()
    {
        throw new System.NotImplementedException();
    }

    public override bool Swipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward)
    {
        interactionPositionNavigationObject.Navigate(targetNavigationPosition.position);
        return true;
    }

    private void OnValidate()
    {
        if (navigationObject_RootObject != null)
        {
            interactionPositionNavigationObject = navigationObject_RootObject.GetComponentInChildren<NavigationObject>();
        }
    }

    public override bool PointerContactStart(Vector3 worldPosition, Vector3 cameraForward)
    {
        throw new System.NotImplementedException();
    }
}
