using System.Collections.Generic;
using UnityEngine;

public class MotorProteinAnimationController : MonoBehaviour
{
    [SerializeField] private Animator motorAnimator;
    [SerializeField] private string stepTriggerName = "TriggerStep";

    public delegate void FootStepAnimationComplete(bool allStepsComplete);
    public event FootStepAnimationComplete OnFootStepComplete;

    [SerializeField] private List<AnimationClip> footStepClips;
    private int stepIndex = 0;
    private float clipLengthBuffer = .25f;

    public int FootStepCLipCount => footStepClips.Count;

    public void PauseIdle()
    {
        motorAnimator.SetTrigger("Still");
    }

    public void ResumeIdle()
    {
        motorAnimator.SetTrigger("Resume");
    }

    public void PlayFootStep()
    {
        motorAnimator.SetTrigger(stepTriggerName);

        if(stepIndex >= footStepClips.Count)
        {
            return;
        }

        Invoke(nameof(FootStepComplete), GetFootStepClipLengthByIndex(stepIndex));
    }

    private float GetFootStepClipLengthByIndex(int stepIndex)
    {
        return footStepClips[stepIndex].length + clipLengthBuffer;
    }

    private void FootStepComplete()
    {
        stepIndex++;

        bool stepsComplete = stepIndex >= footStepClips.Count;

        OnFootStepComplete?.Invoke(stepsComplete);
    }
}
