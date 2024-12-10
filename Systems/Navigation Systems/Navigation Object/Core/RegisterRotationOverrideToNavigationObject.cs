using UnityEngine;

public class RegisterRotationOverrideToNavigationObject : MonoBehaviour
{
    [SerializeField] private string characterTag = "mainCharacter";

    [SerializeField] private GameObject navigationObject_RootObject;
    private NavigationObject navigationObject;

    [SerializeField] private RotationOverrideArea2D rotationOverrideArea2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(characterTag) == false)
        {
            return;
        }

        navigationObject.RegisterRotationOverrideArea(rotationOverrideArea2D);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(characterTag) == false)
        {
            return;
        }

        navigationObject.DeregisterRotationOverrideArea(rotationOverrideArea2D);
    }

    private void Awake()
    {
        navigationObject = navigationObject_RootObject.GetComponentInChildren<NavigationObject>();
    }
}
