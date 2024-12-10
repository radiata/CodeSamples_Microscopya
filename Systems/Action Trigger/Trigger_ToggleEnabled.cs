using UnityEngine;

public class SetEnabled_ActionTrigger : MonoBehaviour
{
    [SerializeField] private string triggeringTag = "mainCharacter";

    [SerializeField] private Behaviour targetComponent;

    [SerializeField] private ChangeActiveState changeActiveState;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(triggeringTag))
        {
            TriggerAction();
        }
    }

    private void TriggerAction()
    {
        switch (changeActiveState)
        {
            case ChangeActiveState.Off:
                targetComponent.enabled = false;
                break;
            case ChangeActiveState.On:
                targetComponent.enabled = true;
                break;
            case ChangeActiveState.Toggle:
                targetComponent.enabled = !targetComponent.enabled;
                break;
        }
    }
}