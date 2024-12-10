using UnityEngine;

public class SynthasePress : MonoBehaviour
{
    [SerializeField] private Animator synthasePressAnimator;
    [SerializeField] private string firstPressTrigger = "FirstPressTrigger";
    [SerializeField] private string loopPressTrigger = "LoopPressTrigger";

    [ContextMenu("Trigger First Press")]
    public void PlayFirstAnimation()
    {
        synthasePressAnimator.SetTrigger(firstPressTrigger);
    }

    [ContextMenu("Trigger Loop Press")]
    public void PlayLoopAnimation()
    {
        synthasePressAnimator.SetTrigger(loopPressTrigger);
    }
}
