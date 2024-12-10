using UnityEngine;

public class AutoTravers_NavigationLinkObject : NavigationObject
{
    [SerializeField] private GameObject navigationObject_rootObject;
    private NavigationObject navigationObject;

    public override Quaternion GetRotationBasedOnLocation(Vector3 location, FacingDirection facingDirection)
    {
        return navigationObject.GetRotationBasedOnLocation(location, facingDirection);
    }

    protected override void Awake()
    {
        navigationObject = navigationObject_rootObject.GetComponentInChildren<NavigationObject>();
    }
}
