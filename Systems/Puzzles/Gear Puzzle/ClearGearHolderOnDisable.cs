using UnityEngine;

public class ClearGearHolderOnDisable : MonoBehaviour
{
    [SerializeField] private Gear gear;

    private void OnDisable()
    {
        gear.UpdateGearHolder(null);
    }
}
