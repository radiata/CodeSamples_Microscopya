using UnityEngine;

public class MapConfinesComponent : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapConfines;

    public PolygonCollider2D MapConfines => mapConfines;
}
