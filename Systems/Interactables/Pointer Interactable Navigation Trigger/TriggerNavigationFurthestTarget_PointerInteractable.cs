using UnityEngine;
using UnityEngine.AI;

public class TriggerNavigationFurthestTarget_PointerInteractable : PointerInteractable_Base
{
    [SerializeField] private CharacterNavigationManager characterNavigationManager;

    [SerializeField] private Transform targetNavigationPosition_One;
    [SerializeField] private Transform targetNavigationPosition_Two;

    [SerializeField] private GameObject navigationObject_RootObject_One;
    private NavigationObject interactionPositionNavigationObject_One;

    [SerializeField] private GameObject navigationObject_RootObject_Two;
    private NavigationObject interactionPositionNavigationObject_Two;

    public override int PriorityValue() => PointerInteractable_References.NavigationTrigger;

    public override bool IsClickable() => true;
    public override bool IsDraggable() => false;
    public override bool IsHoldable() => true;
    public override bool IsSwipeable() => true;
    public override bool IsPointerContactStartable() => false;

    public override bool SendPassThrough() => true;
    public override bool ReceivePassThrough() => true;

    public override PointerInteractable_Base HoldStart(Vector3 worldPosition, out bool consumed)
    {
        consumed = NavigateToFurthestPoint();
        return this;
    }

    public override void Holding(Vector3 worldPosition, Vector3 cameraForward)
    {
        return;
    }

    public override void HoldEnd(Vector3 worldPosition)
    {
        NavigateToFurthestPoint();
    }

    public override bool Click(Vector3 worldPosition, Vector3 cameraForward)
    {
        return NavigateToFurthestPoint();
    }

    public override void Drag()
    {
        throw new System.NotImplementedException();
    }

    public override bool Swipe(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 cameraForward)
    {
        return NavigateToFurthestPoint();
    }

    public override bool PointerContactStart(Vector3 worldPosition, Vector3 cameraForward)
    {
        throw new System.NotImplementedException();
    }

    private bool NavigateToFurthestPoint()
    {
        NavMeshPath navMeshPath_One;
        bool isValid_PathOne = characterNavigationManager.EvaluatePath(targetNavigationPosition_One.position, false, out navMeshPath_One);

        NavMeshPath navMeshPath_Two;
        bool isValid_PathTwo = characterNavigationManager.EvaluatePath(targetNavigationPosition_Two.position, false, out navMeshPath_Two);

        if (isValid_PathOne == true)
        {
            if (isValid_PathTwo == true)
            {
                var d1 = NavMeshPathUtilities.GetPathRemainingDistance(navMeshPath_One);
                var d2 = NavMeshPathUtilities.GetPathRemainingDistance(navMeshPath_Two);
                if (d1 < d2)
                {
                    characterNavigationManager.OnNavigate(navMeshPath_Two);
                    return true;
                }
                else if (d2 < d1)
                {
                    characterNavigationManager.OnNavigate(navMeshPath_One);
                    return true;
                }
            }
            else
            {
                interactionPositionNavigationObject_Two.Navigate(targetNavigationPosition_Two.position, true);
                return true;
            }
        }
        else if(isValid_PathTwo == true)
        {
            interactionPositionNavigationObject_One.Navigate(targetNavigationPosition_One.position, true);
            return true;
        }

        return false;
    }

    private void Awake()
    {
        if (navigationObject_RootObject_One != null)
        {
            interactionPositionNavigationObject_One = navigationObject_RootObject_One.GetComponentInChildren<NavigationObject>();
        }

        if (navigationObject_RootObject_Two != null)
        {
            interactionPositionNavigationObject_Two = navigationObject_RootObject_Two.GetComponentInChildren<NavigationObject>();
        }
    }
}
