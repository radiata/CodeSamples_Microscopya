using UnityEngine;

public class NavigateToTransform : MonoBehaviour
{
    [SerializeField] private Transform targetLocation;
    [SerializeField] private NavigationObject navigationObject;

    [ContextMenu("Move to Target")]
    public void NavigateToTarget()
    {
        navigationObject.Navigate(targetLocation.position, true);
    }
}
