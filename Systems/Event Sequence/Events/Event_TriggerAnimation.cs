using UnityEngine;

public class Event_TriggerAnimation : Base_Event
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animationState;

    internal override void HandleEvent()
    {
        animator.Play(animationState);
        CompleteEvent();
    }
}
