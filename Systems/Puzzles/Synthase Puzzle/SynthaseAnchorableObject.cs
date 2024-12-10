using UnityEngine;

public class SynthaseAnchorableObject : MonoBehaviour
{
    [SerializeField] private SynthaseAnchorableType synthaseAnchorableType;
    [SerializeField] private Transform defaultParent;

    private SynthaseReceiver currentReceiver;

    public SynthaseAnchorableType SynthaseAnchorableType => synthaseAnchorableType;
    
    public bool isReceiverNull => currentReceiver == null;

    public void SetAnchor(Transform anchorTransform)
    {
        transform.SetParent(anchorTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void ResetParent()
    {
        transform.SetParent(defaultParent);
    }

    public void UpdateSynthaseReceiver(SynthaseReceiver synthaseReceiver)
    {
        if (currentReceiver != null)
        {
            currentReceiver.ClearAnchoredPuzzlePiece();
        }
        currentReceiver = synthaseReceiver;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
        Destroy(defaultParent.gameObject);
    }
}
