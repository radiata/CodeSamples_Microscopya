using UnityEngine;

public class NavigationObject_SetupAdditionalData : MonoBehaviour
{
    [SerializeField] private int sortingOrder;
    public int SortingOrder => sortingOrder;

    [SerializeField] private NavigationObjectCameraData navigationObjectCameraData;
    public NavigationObjectCameraData NavigationObjectCameraData => navigationObjectCameraData;

    [SerializeField] private NavigationObject navigationObject;

    private void OnValidate()
    {
        if (navigationObject == null)
        {
            navigationObject = GetComponentInChildren<NavigationObject>();
        }

        if(navigationObject != null)
        {
            AssignSortingOrder();
            AssignCameraData();
        }
    }

    private void AssignSortingOrder()
    {
        navigationObject.SetCharacterSortingOrder(sortingOrder);
    }

    private void AssignCameraData()
    {
        navigationObject.SetCameraData(navigationObjectCameraData);
    }
}
