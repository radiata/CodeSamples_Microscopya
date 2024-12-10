using UnityEngine;

public class SwitchResponder_Reference : MonoBehaviour
{
    [SerializeField] private GameObject switchResponder_GameObject;
    private I_SwitchResponder switchResponder;

    public I_SwitchResponder GetSwitchResponder() => switchResponder;

    private void Awake()
    {
        switchResponder = switchResponder_GameObject.GetComponent<I_SwitchResponder>();
    }
}
