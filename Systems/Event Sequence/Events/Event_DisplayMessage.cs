using UnityEngine;

public class Event_DisplayMessage : Base_Event
{
    [SerializeField] private TranslatableText_SO messageText;

    [SerializeField] private float displayTime;

    internal override void HandleEvent()
    {
        Message.Instance.DisplayMessage(messageText, displayTime);
        CompleteEvent();
    }
}
