using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSignPostController : MonoBehaviour
{
    [SerializeField] private List<ObjectiveSignPost> objectiveSignPosts = new List<ObjectiveSignPost>();

    private void OnEnable()
    {
        Objectives.OnObjectiveCompleted += OnObjectiveCompleted;
        HintState.OnHintStateChanged += OnHintStateChanged;
    }

    private void OnDisable()
    {
        Objectives.OnObjectiveCompleted -= OnObjectiveCompleted;
        HintState.OnHintStateChanged -= OnHintStateChanged;
    }

    private void OnHintStateChanged(bool isEnabled)
    {
        foreach (ObjectiveSignPost signPost in objectiveSignPosts)
        {
            signPost.DisableSignPost();
        }

        if (HintState.HintsEnabledState == true)
        {
            Objectives.Instance.CurrentObjective.ObjectiveSignPost.EnableSignPost();
        }
    }

        private void OnObjectiveCompleted()
    {
        foreach (ObjectiveSignPost signPost in objectiveSignPosts)
        {
            signPost.DisableSignPost();
        }

        if (HintState.HintsEnabledState == true)
        {
            Objectives.Instance.CurrentObjective.ObjectiveSignPost.EnableSignPost();
        }
    }
}
