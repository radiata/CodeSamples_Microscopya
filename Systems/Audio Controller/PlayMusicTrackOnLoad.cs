using UnityEngine;

public class PlayMusicTrackOnLoad : MonoBehaviour
{
    [SerializeField] private MusicTrack musicTrack;
    [SerializeField] private bool loopTrack;

    [SerializeField] private bool doNotPlayIfQueued = false;

    private void Awake()
    {
        if(doNotPlayIfQueued == true)
        {
            if(AudioController.Instance.QueuedMusicTrack == musicTrack)
            {
                return;
            }
        }

        AudioController.Instance.PlayMusic(musicTrack, loopTrack);
    }
}
