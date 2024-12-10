using UnityEngine;

public class DestroyAnchoredPuzzlePiece : MonoBehaviour
{
    [SerializeField] private SynthaseReceiver synthaseReceiver;

    private void OnDisable()
    {
        synthaseReceiver.DestroyAnchoredPuzzlePiece();
        Destroy(this);
        synthaseReceiver.SetReceiverInactive();
    }
}
