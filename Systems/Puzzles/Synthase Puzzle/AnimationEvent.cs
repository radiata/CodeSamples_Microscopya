using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] private AudioSource clip;

    public void PlaySound()
    {
        clip.Play() ;
    }
}
