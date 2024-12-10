using UnityEngine;

public class RotationOverrideArea2D : MonoBehaviour
{
    [SerializeField] private Vector3 rotationValue_LeftFacing;
    [SerializeField] private Vector3 rotationValue_RightFacing;
    [SerializeField] private Collider2D bounds;

    public Vector3 GetRotationValue(bool facingLeft) => facingLeft ? rotationValue_LeftFacing : rotationValue_RightFacing;

    public bool IsInOverrideArea(Vector3 worldPosition)
    {
        return bounds.OverlapPoint(worldPosition);
    }
}
