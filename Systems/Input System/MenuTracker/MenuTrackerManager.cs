using System.Collections.Generic;
using UnityEngine;

public class MenuTrackerManager : MonoBehaviour
{
    private List<BaseMenuTracker> menuTrackers = new List<BaseMenuTracker>();

    public bool IsAnyMenuOpen => menuTrackers.Count > 0 ? true : false;

    private void OnEnable()
    {
        BaseMenuTracker.OnMenuStateChange += OnMenuStateChange;
    }

    private void OnDisable()
    {
        BaseMenuTracker.OnMenuStateChange -= OnMenuStateChange;
    }

    private void OnMenuStateChange(BaseMenuTracker menuTracker, bool isOpen)
    {
        if (isOpen)
        {
            if (menuTrackers.Contains(menuTracker))
            {
                return;
            }
            menuTrackers.Add(menuTracker);
        }
        else
        {
            menuTrackers.Remove(menuTracker);
        }
    }
}
