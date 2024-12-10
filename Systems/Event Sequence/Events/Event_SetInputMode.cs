using UnityEngine;

public class Event_SetInputMode : Base_Event
{
    [SerializeField] private InputModes inputMode = InputModes.Uninitialized;

    internal override void HandleEvent()
    {
        InputHandler.Instance.ChangeInputMode(inputMode);

        CompleteEvent();
    }
}
