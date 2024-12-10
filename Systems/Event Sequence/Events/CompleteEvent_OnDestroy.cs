using UnityEngine;

public class CompleteEvent_OnDestroy : MonoBehaviour
{
    [SerializeField] private Base_Event eventToComplete;

    private void OnDestroy()
    {
        eventToComplete.CompleteEvent();
    }
}
