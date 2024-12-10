using UnityEngine;

public class RotationResponder_Reference : MonoBehaviour
{
    [SerializeField] private GameObject rotationResponder_GameObject;
    private I_RotationResponder rotationResponder;

    public I_RotationResponder GetRotationResponder() => rotationResponder;

    private void Awake()
    {
        rotationResponder = rotationResponder_GameObject.GetComponent<I_RotationResponder>();
    }
}
