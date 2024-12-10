using System.Collections.Generic;
using UnityEngine;

public class RibosomeAnimator : MonoBehaviour
{
    [SerializeField] private List<Animator> ribosomeAnimators;
    [SerializeField] private string inAutomationFlag = "InAutomation";
    [SerializeField] private string fadeInTrigger = "FadeInTrigger";
    [SerializeField] private string fadeOutTrigger = "FadeOutTrigger";

    [SerializeField] private float fadeTimer = 1.25f;

    public float FadeTimer => fadeTimer;

    public void FadeIn()
    {
        foreach (Animator animator in ribosomeAnimators)
        {
            animator.SetTrigger(fadeInTrigger);
        }
    }

    public void FadeOut()
    {
        foreach (Animator animator in ribosomeAnimators)
        {
            animator.SetTrigger(fadeOutTrigger);
        }
    }

    public void StartAutomation()
    {
        foreach (Animator animator in ribosomeAnimators)
        {
            animator.SetBool(inAutomationFlag, true);
            animator.SetTrigger(fadeInTrigger);
        }
    }
}
