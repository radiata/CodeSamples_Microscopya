using UnityEngine;

public class Event_TextOverlay : Base_Event
{
    [SerializeField] private GameObject typeWriter;

    internal override void HandleEvent()
    {
        typeWriter.SetActive(true);
    }
}
