using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] private GearType gearType;
    [SerializeField] private GearReceiver initialGearHolder;
    [SerializeField] private GearReceiver currentGearHolder;
    [SerializeField] private DragAndDrop dragAndDrop;
    [SerializeField] private GearRotation gearRotation;
    [SerializeField] private GearPhysics gearPhysics;
    [SerializeField] private Collider2D gearCollider;

    private float resetHeightThreshold = -6.2f;

    public GearType GearType => gearType;

    public void ResetGearPosition()
    {
        initialGearHolder.ReceiveItem(dragAndDrop.gameObject);
        gearPhysics.enabled = false;
        StopGearRotation();
    }

    public void SetPosition(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }

    public void StartGearRotation()
    {
        gearRotation.enabled = true;
    }

    public void StopGearRotation()
    {
        gearRotation.enabled = false;
    }

    public void UpdateGearHolder(GearReceiver gearReceiver)
    {
        if (currentGearHolder != null)
        {
            currentGearHolder.ClearAnchoredGear();
        }
        currentGearHolder = gearReceiver;
    }

    private void FixedUpdate()
    {
        if (transform.localPosition.y < resetHeightThreshold)
        {
            ResetGearPosition();
        }
    }

    public void RemoveInteractivity()
    {
        gameObject.layer = LayerReferences.NonInteractableLayer;
        gearCollider.gameObject.layer = LayerReferences.NonInteractableLayer;
        Destroy(dragAndDrop);
    }
}
