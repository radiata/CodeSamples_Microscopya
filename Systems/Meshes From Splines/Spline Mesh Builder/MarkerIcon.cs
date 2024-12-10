using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerIcon : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "CircleMask.png", true);
    }
}
