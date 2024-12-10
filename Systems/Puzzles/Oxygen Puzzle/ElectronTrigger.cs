using UnityEngine;

public class ElectronTrigger : MonoBehaviour
{
    [SerializeField] private ChamberPower chamberPower;
    [SerializeField] private string electronTag = "Electron";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(electronTag) == true)
        {
            chamberPower.ChangePower(1);
        }
    }
}
