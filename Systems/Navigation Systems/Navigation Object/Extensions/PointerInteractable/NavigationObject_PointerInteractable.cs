using System.Collections.Generic;
using UnityEngine;

public class NavigationObject_PointerInteractable : PointerInteractable_Base
{
    [SerializeField] private NavigationObject navigationObject;
    [SerializeField] private bool zPositionMatching = true;

    public void SetNavigationObject(NavigationObject newNavigationObject)
    {
        navigationObject = newNavigationObject;
    }

    public override int PriorityValue() => PointerInteractable_References.NavigationObject;

    public override bool IsClickable() => true;
    public override bool IsDraggable() => false;
    public override bool IsHoldable() => true;
    public override bool IsSwipeable() => true;
    public override bool IsPointerContactStartable() => false;

    public override bool SendPassThrough() => false;
    public override bool ReceivePassThrough() => true;

    public override bool Click(Vector3 worldPosition, Vector3 cameraForward)
    {
        NavigationObject_PointerInteractable navItem = null;
        PointerInteractable_Base[] stack = GetItemStack(worldPosition, cameraForward, PointerInteractable_References.CharacterNavigationLayers);
        List<NavigationObject_PointerInteractable> filteredStack = new List<NavigationObject_PointerInteractable>();
        List<TriggerNavigationFurthestTarget_PointerInteractable> secondaryStack = new List<TriggerNavigationFurthestTarget_PointerInteractable>();

        foreach (PointerInteractable_Base item in stack)
        {
            if (item is NavigationObject_PointerInteractable)
            {
                filteredStack.Add(item as NavigationObject_PointerInteractable);
            }
            else if (item is TriggerNavigationFurthestTarget_PointerInteractable)
            {
                secondaryStack.Add(item as TriggerNavigationFurthestTarget_PointerInteractable);
            }
        }

        if (filteredStack.Count + secondaryStack.Count == 0)
        {
            return false;
        }

        if (filteredStack.Count + secondaryStack.Count == 1)
        {
            if (filteredStack.Count == 1)
            {
                return AttemptClick(filteredStack[0], worldPosition);
            }
            else
            {
                return secondaryStack[0].Click(worldPosition, cameraForward);
            }
        }

        if (filteredStack.Count > 0)
        {
            navItem = filteredStack[0];

            for (int i = 0; i < filteredStack.Count; i++)
            {
                if (filteredStack[i].navigationObject == CharacterNavigationObjectReporter.CachedNavigationObject)
                {
                    navItem = filteredStack[i];
                    break;
                }
            }

            if (AttemptClick(navItem, worldPosition) == true)
            {
                return true;
            }

            for (int i = 0; i < filteredStack.Count; i++)
            {
                navItem = filteredStack[i];
                if (AttemptClick(navItem, worldPosition) == true)
                {
                    return true;
                }
            }
        }

        foreach (TriggerNavigationFurthestTarget_PointerInteractable item in secondaryStack)
        {

            if (item.Click(worldPosition, cameraForward) == true)
            {
                return true;
            }
        }

        return false;
    }

    public virtual bool AttemptClick(NavigationObject_PointerInteractable navItem, Vector3 worldPosition)
    {
        if (zPositionMatching)
        {
            worldPosition.z = navItem.navigationObject.ZPosition;
        }

        navItem.navigationObject.Navigate(worldPosition);
        return true;
    }

    public override void Drag()
    {
        throw new System.NotImplementedException();
    }

    public override PointerInteractable_Base HoldStart(Vector3 worldPosition, out bool consumed)
    {
        consumed = true;
        return this;
    }

    public override void Holding(Vector3 worldPosition, Vector3 cameraForward)
    {
        Click(worldPosition, cameraForward);
    }

    public override void HoldEnd(Vector3 worldPosition)
    {
        return;
    }

    public override bool Swipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward)
    {
        Click(worldEndPosition, cameraForward);
        return true;
    }

    public override bool PointerContactStart(Vector3 worldPosition, Vector3 cameraForward)
    {
        throw new System.NotImplementedException();
    }
}
