using UnityEngine;

public class WarpTo : MonoBehaviour
{
    [SerializeField] private Vector3 warpPointOne;
    [SerializeField] private Vector3 warpPointTwo;
    [SerializeField] private Vector3 warpPointThree;

    [SerializeField] private Transform targetToWarp;

    private void WarpToPoint(int index)
    {
        switch (index)
        {
            case 1:
                targetToWarp.position = warpPointOne;
                break;
            case 2:
                targetToWarp.position = warpPointTwo;
                break;
            case 3:
                targetToWarp.position = warpPointThree;
                break;
        }
    }

    [ContextMenu("Warp to One")]
    private void WarpOne()
    {
        WarpToPoint(1);
    }

    [ContextMenu("Warp to Two")]
    private void WarpTwo()
    {
        WarpToPoint(2);
    }

    [ContextMenu("Warp to Three")]
    private void WarpThree()
    {
        WarpToPoint(3);
    }
}
