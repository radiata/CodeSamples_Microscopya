using UnityEngine;

public class ObjectiveData : MonoBehaviour
{
    [SerializeField] private TranslatableText_SO translatableText;
    [SerializeField] private ObjectiveSignPost objectiveSignPost;

    private int priority = 0;
    private bool complete = false;

    public int Priority => priority;
    public TranslatableText_SO TranslatableText => translatableText;
    public ObjectiveSignPost ObjectiveSignPost => objectiveSignPost;

    public void SetPriority(int priority)
    {
        this.priority = priority;
    }

    public void SetComplete()
    {
        complete = true;
        Objectives.Instance.ObjectiveComplete(this);
    }
}
