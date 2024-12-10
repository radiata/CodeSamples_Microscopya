using UnityEngine;

public class GearRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    public void Update()
    {
        transform.Rotate(transform.forward * rotationSpeed * Time.deltaTime);
    }
}
