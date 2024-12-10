using UnityEngine;

public class ClickResponder_Reference : MonoBehaviour
{
    [SerializeField] private GameObject clickResponder_GameObject;
    private I_ClickResponder clickResponder;

    public I_ClickResponder GetClickResponder() => clickResponder;

    private void Awake()
    {
        clickResponder = clickResponder_GameObject.GetComponent<I_ClickResponder>();
    }
}
