using UnityEngine;
using UnityEngine.Splines;

public class AssignComponents
{
    public static void SetupComponents(GameObject gameObject, Vector2[] polygon2DVerts, SplineContainer corePath, SplineContainer upperPath, SplineContainer lowerPath)
    {
        AssignPolygon2DVerts(gameObject, polygon2DVerts);
        AssignNavigationObject(gameObject, corePath, upperPath, lowerPath);
        AssignPointerInteractable(gameObject);
    }

    private static void AssignPolygon2DVerts(GameObject gameObject, Vector2[] polygon2DVerts)
    {
        PolygonCollider2D polygonCollider2D = gameObject.GetComponentInChildren<PolygonCollider2D>();
        polygonCollider2D.points = polygon2DVerts;
    }

    private static void AssignNavigationObject(GameObject gameObject, SplineContainer corePath, SplineContainer upperPath, SplineContainer lowerPath)
    {
        var navigationObject = gameObject.GetComponentInChildren<NavigationObject>();
        if (navigationObject == null)
        {
            return;
        }

        navigationObject.SetSplinePaths(corePath, upperPath, lowerPath);

    }

    private static void AssignPointerInteractable(GameObject gameObject)
    {
        var pointerInteractable = gameObject.GetComponentInChildren<NavigationObject_PointerInteractable>();
        if(pointerInteractable == null)
        {
            pointerInteractable = gameObject.transform.parent.GetComponentInChildren<NavigationObject_PointerInteractable>();

            if (pointerInteractable == null)
            {
                return;
            }
        }

        var navigationObject = gameObject.transform.parent.GetComponentInChildren<NavigationObject>();
        if (navigationObject == null)
        {
            return;
        }
        pointerInteractable.SetNavigationObject(navigationObject);
    }
}
