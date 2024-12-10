using UnityEngine;

public class TrophyAnimationTargets : MonoBehaviour
{
    [SerializeField] private RectTransform startAnchor;
    [SerializeField] private RectTransform endAnchor;

    public Vector2 StartPosition => startAnchor.position;
    public Vector2 EndPosition => endAnchor.position;
}
