using UnityEngine;

public class UpdateImageOnEnable : MonoBehaviour
{
    [SerializeField] private ImageRelativePositioning imageRelativePositioning;

    private void OnEnable()
    {
        imageRelativePositioning.UpdatePosition();
    }
}
