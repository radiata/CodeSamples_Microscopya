using System.Collections.Generic;
using UnityEngine;

public class AutomatedTRNA : MonoBehaviour
{
    [SerializeField] private SimpleTRNA stopCodon;

    [SerializeField] private SimpleTRNA[] pool;
    private int poolIndex = 0;

    private List<SimpleTRNA> inUse = new List<SimpleTRNA>();

    [ContextMenu("Spawn SimpleTRNA")]
    //Returns time to destination
    public float? SpawnSimpleTRNA()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[poolIndex].isInUse == true)
            {
                poolIndex = (poolIndex + 1) % pool.Length;
            }
            else
            {
                pool[poolIndex].Spawn();
                inUse.Add(pool[poolIndex]);
                return pool[poolIndex].SpawnTimeToDestination;
            }
        }

        return null;
    }

    public float? SpawnStopCodon()
    {
        stopCodon.Spawn();
        return stopCodon.SpawnTimeToDestination;
    }

    public void DespawnStopCodon()
    {
        stopCodon.Despawn();

        for (int i = 0; i < inUse.Count; i++)
        {
            inUse[i].Despawn();
        }
    }

    [ContextMenu("Despawn TRNA")]
    public void DespawnOldest()
    {
        inUse[0].Despawn();
        inUse.RemoveAt(0);
    }
}
