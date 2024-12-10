using UnityEngine;

public abstract class Base_NegativeFeedback : MonoBehaviour
{
    public delegate void OnNegativeFeedbackStartEvent();
    public OnNegativeFeedbackStartEvent OnNegativeFeedbackStart;

    public delegate void OnNegativeFeedbackEndEvent();
    public OnNegativeFeedbackEndEvent OnNegativeFeedbackEnd;

    public abstract void ExecuteNegativeFeedback();

    public void StartNegativeFeedback()
    {
        OnNegativeFeedbackStart?.Invoke();
    }

    public void EndNegativeFeedback()
    {
        OnNegativeFeedbackEnd?.Invoke();
    }
}
