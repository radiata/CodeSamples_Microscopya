using UnityEngine;

public class AnimationEventTriggers : MonoBehaviour
{
    [SerializeField] private GameObject objToActivate;

    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private float audioReductionStep = .15f;
    [SerializeField] private int ignoreSteps = 3;
    private int stepCounter = 0;

    public void PlaySound()
    {
        soundEffect.Play();
        if (stepCounter >= ignoreSteps)
        { soundEffect.volume = Mathf.Clamp01(soundEffect.volume - audioReductionStep); }
        stepCounter++;
    }

    public void EndMenu()
    {
        objToActivate.SetActive(true);
    }
}
