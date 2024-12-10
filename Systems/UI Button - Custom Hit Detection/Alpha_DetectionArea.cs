using UnityEngine;
using UnityEngine.UI;

public class Alpha_DetectionArea : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private float alphaHitDetection = .1f;

    private void Start()
    {
        buttonImage.alphaHitTestMinimumThreshold = alphaHitDetection;
    }
}
