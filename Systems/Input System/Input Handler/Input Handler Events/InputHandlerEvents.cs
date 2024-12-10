public static class InputHandlerEvents
{
    public delegate void SkipMultiTouchDelayEvent();
    public static event SkipMultiTouchDelayEvent OnSkipMultiTouchDelay;

    public static void RaiseSkipMultiTouchDelayEvent()
    {
        OnSkipMultiTouchDelay?.Invoke();
    }
}
