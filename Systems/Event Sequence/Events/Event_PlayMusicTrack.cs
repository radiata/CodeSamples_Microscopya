using UnityEngine;

public class Event_PlayMusicTrack : Base_Event
{
    [SerializeField] private MusicTrack musicTrack;
    [SerializeField] private bool loopTrack;

    [SerializeField] private bool doNotPlayIfQueued = false;

    internal override void HandleEvent()
    {
        if (doNotPlayIfQueued == true)
        {
            if (AudioController.Instance.QueuedMusicTrack == musicTrack)
            {
                return;
            }
        }

        AudioController.Instance.PlayMusic(musicTrack, loopTrack);

        CompleteEvent();
    }
}
