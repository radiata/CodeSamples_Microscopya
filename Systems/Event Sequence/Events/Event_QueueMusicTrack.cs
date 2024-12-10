using UnityEngine;

public class Event_QueueMusicTrack : Base_Event
{
    [SerializeField] private MusicTrack musicTrack;
    [SerializeField] private bool loopTrack;
    [SerializeField] private float delayBetweenSongs = 0f;

    internal override void HandleEvent()
    {

        if (AudioController.Instance.QueuedMusicTrack == musicTrack)
        {
            return;
        }


        AudioController.Instance.QueueMusicTrack(musicTrack, loopTrack, delayBetweenSongs);

        CompleteEvent();
    }
}
