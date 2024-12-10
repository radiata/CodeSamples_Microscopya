using UnityEngine;

public class ClearSynthaseReceiverOnDisable : MonoBehaviour
{
    [SerializeField] private SynthaseAnchorableObject synthaseAnchorableObject;

    private void OnEnable()
    {
        if(synthaseAnchorableObject.isReceiverNull == true)
        {
            synthaseAnchorableObject.ResetParent();
        }
    }
    private void OnDisable()
    {
        synthaseAnchorableObject.UpdateSynthaseReceiver(null);
    }
}
