public class Event_ClearObjectives : Base_Event
{
    internal override void HandleEvent()
    {
        Objectives.Instance.ClearObjectives();
        CompleteEvent();
    }
}
