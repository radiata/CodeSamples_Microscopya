using UnityEngine;

public class OxygenPuzzleMolecules : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animationTriggerStep = "TriggerNextStep";
    [SerializeField] private string animationTriggerLoop = "TriggerLoop";
    [SerializeField] private string animationJumpToLoop = "JumpToLoop";

    [ContextMenu("Trigger Next Step")]
    public void NextAnimationState()
    {
        animator.SetTrigger(animationTriggerStep);
    }

    [ContextMenu("Start Loop")]
    public void StartLoop()
    {
        animator.SetTrigger(animationTriggerLoop);
    }

    public void JumpToLoop()
    {
        animator.SetTrigger(animationJumpToLoop);
    }
}
