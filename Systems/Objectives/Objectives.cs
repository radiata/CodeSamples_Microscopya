using System.Collections.Generic;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    public static Objectives Instance;

    [SerializeField] private ObjectivesDisplay objectivesDisplay;

    private List<ObjectiveData> completeObjectives = new List<ObjectiveData>();

    public delegate void OnObjectiveCompleteEvent();
    public static OnObjectiveCompleteEvent OnObjectiveCompleted;

    private ObjectiveData currentObjective;
    public ObjectiveData CurrentObjective => currentObjective;

    public void ClearObjectives()
    {
        completeObjectives.Clear();
        objectivesDisplay.ClearObjectiveDisplay();
    }

    internal void ObjectiveComplete(ObjectiveData objectiveData)
    {
        completeObjectives.Add(objectiveData);

        currentObjective = GetHighestPriorityObjective(completeObjectives);
        UpdateObjectiveDisplay(currentObjective);

        OnObjectiveCompleted?.Invoke();
    }

    private ObjectiveData GetHighestPriorityObjective(List<ObjectiveData> objectiveDataList)
    {
        if (objectiveDataList.Count <= 0)
        {
            return null;
        }

        ObjectiveData highest = objectiveDataList[0];

        for (int i = 1; i < objectiveDataList.Count; i++)
        {
            if (objectiveDataList[i].Priority > highest.Priority)
            {
                highest = objectiveDataList[i];
            }
        }

        return highest;
    }

    private void UpdateObjectiveDisplay(ObjectiveData objectiveData)
    {
        if(objectivesDisplay.CurrentTranslatableText == objectiveData.TranslatableText)
        {
            return;
        }

        objectivesDisplay.UpdateObjectiveDisplay(objectiveData.TranslatableText);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }
}
