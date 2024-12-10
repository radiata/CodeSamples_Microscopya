using System.Collections.Generic;
using UnityEngine;

public class ObjectiveOrder : MonoBehaviour
{
    [SerializeField] private List<ObjectiveData> objectiveDataList = new List<ObjectiveData>();

    private void Awake()
    {
        AssignPriorities();
    }

    private void OnEnable()
    {
        AssignPriorities();
    }

    private void AssignPriorities()
    {
        for (int i = 0; i < objectiveDataList.Count; i++)
        {
            objectiveDataList[i].SetPriority(i);
        }
    }
}
