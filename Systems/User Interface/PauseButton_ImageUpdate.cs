using UnityEngine;

public class PauseButton_ImageUpdate : MonoBehaviour
{
    [SerializeField] private PauseButton pauseButton;
    private void OnEnable()
    {
        pauseButton.UpdateImage();
    }
}
