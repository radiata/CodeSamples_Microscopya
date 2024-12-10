using UnityEngine;

public class GearPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;

    private void OnEnable()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.freezeRotation = false;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.freezeRotation = true;
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
}
