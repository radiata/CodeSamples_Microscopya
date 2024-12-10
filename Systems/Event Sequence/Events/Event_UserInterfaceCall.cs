using UnityEngine;

public class Event_UserInterfaceCall : Base_Event
{
    [SerializeField] private UserInterfaceLayout interfaceLayout; 

    internal override void HandleEvent()
    {
        UserInterface.Instance.ChangeUserInterfaceLayout(interfaceLayout);
        CompleteEvent();
    }
}
