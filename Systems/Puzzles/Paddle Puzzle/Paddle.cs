using UnityEngine;

public class Paddle : MonoBehaviour, I_SwitchResponder
{
    [SerializeField] private InteractableSwitch interactableSwitch;
    [SerializeField] private Collider switchCollider;

    private bool switchOn;

    public delegate void PaddleSwitchedEvent();
    public PaddleSwitchedEvent OnPaddleSwitched;

    public bool isSolved() => switchOn;

    public void InitializePaddle(bool solved)
    {
        if(solved == true)
        {
            interactableSwitch.InitializeState(SwitchState.On);
            switchOn = true;
        }
        else
        {
            interactableSwitch.InitializeState(SwitchState.Off);
            switchOn = false;
        }
    }

    public void SwitchIsBusy()
    {
        switchOn = false;
        OnPaddleSwitched?.Invoke();
    }

    public void SwitchIsOn()
    {
        switchOn = true;
        OnPaddleSwitched?.Invoke();
    }

    public void SwitchIsOff()
    {
        switchOn = false;
        OnPaddleSwitched?.Invoke();
    }

    public void EnableInteractivity()
    {
        switchCollider.gameObject.layer = LayerReferences.InteractablePuzzleObjectsLayer;
        
    }

    public void DisableInteractivity()
    {
        switchCollider.gameObject.layer = LayerReferences.NonInteractableLayer;
    }

    public void RemoveInteractivity()
    {
        switchCollider.gameObject.layer = LayerReferences.NonInteractableLayer;
        Destroy(interactableSwitch);
    }
}
