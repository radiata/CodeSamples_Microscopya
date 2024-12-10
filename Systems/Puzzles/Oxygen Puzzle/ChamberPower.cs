using System.Collections;
using UnityEngine;

public class ChamberPower : MonoBehaviour
{
    [SerializeField] private float transitionTime;
    [SerializeField] private Revealable halfLight;
    [SerializeField] private Revealable fullLight;

    private Coroutine halfPowerRoutine;
    private Coroutine fullPowerRoutine;
    private PowerState powerState = 0;
    private PowerState targetState;

    private bool soundEnabled = false;

    public delegate void PowerChangeEvent(PowerState PowerState);
    public event PowerChangeEvent OnPowerChange;

    public PowerState PowerState => powerState;

    public bool IsBusy()
    {
        return !(halfPowerRoutine == null && fullPowerRoutine == null);
    }

    public void SetSound(bool enabled)
    {
        soundEnabled = enabled;
    }

    public void ChangePower(int changeAmount)
    {
        PowerState x = (PowerState)Mathf.Clamp((float)powerState + (float)changeAmount, 0, 3);
        ChangePower(x);
    }

    public void ChangePower(PowerState newPowerState)
    {
        if (IsBusy() == true || newPowerState == powerState)
        {
            return;
        }

        targetState = newPowerState;

        switch (newPowerState)
        {
            case PowerState.None:
                SetNoPower();
                break;
            case PowerState.Half:
                SetHalfPower();
                break;
            case PowerState.Full:
                SetFullPower();
                break;
        }
    }

    private void SetNoPower()
    {
        switch (powerState)
        {
            case PowerState.None:
                return;
            case PowerState.Half:
                halfPowerRoutine = StartCoroutine(PowerChange(halfLight, false));
                break;
            case PowerState.Full:
                halfPowerRoutine = StartCoroutine(PowerChange(halfLight, false));
                fullPowerRoutine = StartCoroutine(PowerChange(fullLight, false));
                break;
        }

        Invoke(nameof(UpdateState), transitionTime);
    }

    private void SetHalfPower()
    {
        switch (powerState)
        {
            case PowerState.None:
                PlayAudio();
                halfPowerRoutine = StartCoroutine(PowerChange(halfLight, true));
                break;
            case PowerState.Half:
                return;
            case PowerState.Full:
                fullPowerRoutine = StartCoroutine(PowerChange(fullLight, false));
                break;
        }

        Invoke(nameof(UpdateState), transitionTime);
    }

    private void SetFullPower()
    {
        switch (powerState)
        {
            case PowerState.None:
                PlayAudio();
                halfPowerRoutine = StartCoroutine(PowerChange(halfLight, true));
                fullPowerRoutine = StartCoroutine(PowerChange(fullLight, true));
                break;
            case PowerState.Half:
                PlayAudio();
                fullPowerRoutine = StartCoroutine(PowerChange(fullLight, true));
                break;
            case PowerState.Full:
                return;
        }

        Invoke(nameof(UpdateState), transitionTime);
    }

    private void UpdateState()
    {
        powerState = targetState;
        halfPowerRoutine = null;
        fullPowerRoutine = null;

        OnPowerChange?.Invoke(powerState);
    }

    private IEnumerator PowerChange(Revealable revealable, bool reveal)
    {
        float normalTime = 0;
        float currentTime = 0;

        if (transitionTime == 0)
        {
            UpdateColor(revealable, 1, reveal);
        }
        else
        {
            while (currentTime < transitionTime)
            {
                normalTime = Mathf.Clamp01(currentTime / transitionTime);

                UpdateColor(revealable, normalTime, reveal);


                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void PlayAudio()
    {
        if (soundEnabled == false)
        {
            return;
        }

        AudioController.Instance.PlaySoundEffect(SoundEffect.OxygenElectronDelivered, false);
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
