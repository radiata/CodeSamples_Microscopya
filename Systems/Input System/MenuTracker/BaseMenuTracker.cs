using UnityEngine;

public class BaseMenuTracker : MonoBehaviour
{
    public delegate void MenuStateChange(BaseMenuTracker menu, bool isOpen);
    public static MenuStateChange OnMenuStateChange;

    protected virtual void OnEnable()
    {
        OpenMenu();
    }

    protected virtual void OnDisable()
    {
        CloseMenu();
    }

    public virtual void OpenMenu()
    {
        OnMenuStateChange?.Invoke(this, true);
    }

    public virtual void CloseMenu()
    {
        OnMenuStateChange?.Invoke(this, false);
    }
}
