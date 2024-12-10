using UnityEngine;

public class StopGearRotationOnDisable : MonoBehaviour
{
    [SerializeField] private Gear gear;

    private void OnDisable()
    {
        gear.StopGearRotation();
    }
}
