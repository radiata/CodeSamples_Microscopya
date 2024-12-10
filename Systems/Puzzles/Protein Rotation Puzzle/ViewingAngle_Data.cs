using System.Collections.Generic;
using UnityEngine;

public static class ViewingAngle_Data
{
    private static List<(Vector3, float)> viewingAngles = new List<(Vector3, float)>()
    {
        (new Vector3(0f, 0f, 0f), 1f),
        (new Vector3(273.879547f, 180f, 192.627457f), 1f),
        (new Vector3(335.509979f, 126.000015f, 266f), 1f),
        (new Vector3(304.47998f, 180.330994f, 179.875015f), 1f),
        (new Vector3(76.7000122f, 5.07997561f, 185.450012f), 1f),
        (new Vector3(348.299988f, 21.3999958f, 314f), 1f),
        (new Vector3(301.23999f, 180.057999f, 179.837997f), 1f),
        (new Vector3(51.4599991f, 42.5999985f, 298.830017f), 1f),
        (new Vector3(299.799988f, 333.400055f, 278.400024f), 1f),
        (new Vector3(38.2000122f, 305.399994f, 356.899994f), 1f),
        //(new Vector3(76.2100067f, 42.2099991f, 347.660004f), 1f),
    };

    public static (Vector3, float) GetViewingData(int index)
    {
        if (index >= viewingAngles.Count)
        {
            Debug.Log("out of bounds index = " + index);
        }
        return viewingAngles[index];
    }
}
