using UnityEngine;

public static class MapConfinesChecker
{
    public static PolygonCollider2D FindMapConfines()
    {
        var confines = GameObject.FindFirstObjectByType<MapConfinesComponent>();
        if (confines != null)
        {
            return confines.MapConfines;
        }
        return null;
    }
}
