using UnityEngine;

public class RegisterSortingOverrideToNavigationObject : MonoBehaviour
{
    [SerializeField] private string characterTag = "mainCharacter";

    [SerializeField] private GameObject navigationObject_RootObject;
    private NavigationObject navigationObject;

    [SerializeField] private SortingOverrideArea2D sortingOverrideArea2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(characterTag) == false)
        {
            return;
        }

        navigationObject.RegisterSortingOverrideArea(sortingOverrideArea2D);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(characterTag) == false)
        {
            return;
        }

        navigationObject.DeregisterSortingOverrideArea(sortingOverrideArea2D);
    }

    private void Awake()
    {
        navigationObject = navigationObject_RootObject.GetComponentInChildren<NavigationObject>();
    }
}
