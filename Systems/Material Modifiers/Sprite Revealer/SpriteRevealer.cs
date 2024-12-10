using System.Collections;
using UnityEngine;

public class SpriteRevealer : MonoBehaviour
{
    [SerializeField] private Revealable[] revealables;
    [SerializeField] private bool revealedState = false;
    [SerializeField] private float transitionTime;

    private Coroutine stateSwitchRoutine;

    //stores last known state of if we were revealing/hiding (true/false) and normalized progress
    private (bool, float) lastRevealProgress;
    //e.g., (false, 0) would be hiding in progress at 0% complete
    //e.g., (false, 1) would be hiding in progress at 100% complete
    //e.g., (true, 0) would be revealing in progress at 0% complete
    //e.g., (true, 1) would be revealing in progress at 100% complete

    public float TransitionTime => transitionTime;

    public void ChangeRevealedState(bool reveal)
    {
        if (stateSwitchRoutine != null)
        {
            StopCoroutine(stateSwitchRoutine);
        }

        float startTimeNormalized = 0;

        if (reveal)
        {
            startTimeNormalized = lastRevealProgress.Item1 == true ? lastRevealProgress.Item2 : 1 - lastRevealProgress.Item2;

            RevealObjects(startTimeNormalized);
        }
        else
        {
            startTimeNormalized = lastRevealProgress.Item1 == false ? lastRevealProgress.Item2 : 1 - lastRevealProgress.Item2;

            HideObjects(startTimeNormalized);
        }
    }

    private void RevealObjects(float startTimeNormalized)
    {
        float startTime = Mathf.Clamp01(startTimeNormalized) * transitionTime;
        stateSwitchRoutine = StartCoroutine(ChangeRevealState(startTime, true));
    }

    private void HideObjects(float startTimeNormalized)
    {
        float startTime = Mathf.Clamp01(startTimeNormalized) * transitionTime;
        stateSwitchRoutine = StartCoroutine(ChangeRevealState(startTime, false));

    }

    private void Start()
    {
        foreach (Revealable revealable in revealables)
        {
            UpdateColor(revealable, 1, revealedState);
        }
        lastRevealProgress = (revealedState, 1);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        stateSwitchRoutine = null;
    }

    private IEnumerator ChangeRevealState(float currentTime, bool reveal)
    {
        float normalTime = 0;

        if (transitionTime > 0)
        {
            while (currentTime < transitionTime)
            {
                normalTime = Mathf.Clamp01(currentTime / transitionTime);

                foreach (Revealable revealable in revealables)
                {
                    UpdateColor(revealable, normalTime, reveal);
                }

                currentTime += Time.deltaTime;
                lastRevealProgress = (reveal, normalTime);
                yield return null;
            }
        }

        normalTime = 1;

        foreach (Revealable revealable in revealables)
        {
            UpdateColor(revealable, normalTime, reveal);
        }
        lastRevealProgress = (reveal, 1);
    }

    private void UpdateColor(Revealable revealable, float lerpPercent, bool reveal)
    {
        if (reveal)
        {
            //lerp to reveal state
            revealable.SpriteRenderer.color =
                Color.Lerp(revealable.HiddenStateColor, revealable.RevealedStateColor, lerpPercent);
        }
        else
        {
            //lerp to hidden state
            revealable.SpriteRenderer.color =
                Color.Lerp(revealable.RevealedStateColor, revealable.HiddenStateColor, lerpPercent);
        }
    }
}