using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour, I_DraggablePuzzlePiece
{
    [SerializeField] private bool navigateToPuzzleOnInteract = true;
    [SerializeField] private PuzzleManager puzzleManager;

    [SerializeField] private bool scaleOnInteract = true;
    [SerializeField] private float scaleMultiplier = 1.5f;
    private Vector3 initialScale;

    [SerializeField] private bool lookForReceiverOnInteractEnd = true;
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private List<GameObject> validReceivers = new List<GameObject>();
    private List<GameObject> receiversInRange = new List<GameObject>();

    [SerializeField] private List<MonoBehaviour> whileInteractingToggleOffComponents = new List<MonoBehaviour>();
    [SerializeField] private List<MonoBehaviour> onReceivedToggleOffComponents = new List<MonoBehaviour>();

    [SerializeField] private bool resolveMultipleReceiversByDistance = true;
    [SerializeField] private bool maintainZDepthWhileInteracting = true;

    [SerializeField] private SoundEffect dropped_Sound;
    [SerializeField] private SoundEffect received_Sound;

    [SerializeField] private bool snapToCursor = false;

    [SerializeField] private Base_NegativeFeedback negativeFeedback;

    private static ContactFilter2D contactFilter = new ContactFilter2D().NoFilter();

    private Vector3 cursorOffset;

    public void AssignValidReceivers(List<GameObject> validReceivers)
    {
        this.validReceivers = validReceivers;
    }

    public void AssignPuzzleManager(PuzzleManager puzzleManager)
    {
        this.puzzleManager = puzzleManager;
    }

    public void OnDragStart(Vector3 worldPosition)
    {
        cursorOffset = transform.position - worldPosition;

        if (navigateToPuzzleOnInteract)
        {
            puzzleManager.Navigate();
        }

        initialScale = transform.localScale;

        if (scaleOnInteract)
        {
            transform.localScale = initialScale * scaleMultiplier;
        }

        ToggleComponents(whileInteractingToggleOffComponents, false);

        UpdateReceiversInRange();
        triggerCollider.isTrigger = true;
    }

    public void WhileDragging(Vector3 worldPosition, Vector3 cameraForward)
    {
        Vector3 newPosition = worldPosition;

        newPosition += snapToCursor == true ? Vector3.zero : cursorOffset;

        if (maintainZDepthWhileInteracting)
        {
            newPosition.z = transform.position.z;
        }

        transform.position = newPosition;
    }

    public void OnDragEnd(Vector3 worldPosition)
    {
        if (scaleOnInteract)
        {
            transform.localScale = initialScale;
        }

        ToggleComponents(whileInteractingToggleOffComponents, true);

        if (lookForReceiverOnInteractEnd)
        {
            ResolveReceiverInteractions();
        }
        else
        {
            AudioController.Instance.PlaySoundEffect(dropped_Sound, false);
        }

        triggerCollider.isTrigger = false;
    }

    public void SimulateDragAndDrop(Vector3 worldPosition)
    {
        ToggleComponents(whileInteractingToggleOffComponents, false);

        UpdateReceiversInRange();
        triggerCollider.isTrigger = true;

        if (maintainZDepthWhileInteracting)
        {
            worldPosition.z = transform.position.z;
        }

        transform.position = worldPosition;

        ToggleComponents(whileInteractingToggleOffComponents, true);

        if (lookForReceiverOnInteractEnd)
        {
            if (resolveMultipleReceiversByDistance == true
            && receiversInRange.Count > 1)
            {
                SortReceiversByDistance();
            }

            foreach (GameObject receiver in receiversInRange)
            {
                ItemReceipt itemReceived = receiver.GetComponent<DragAndDropReceiver_Reference>().GetDragAndDropReceiver().ReceiveItem(gameObject);

                if (itemReceived != ItemReceipt.ItemRejected)
                {
                    ToggleComponents(onReceivedToggleOffComponents, false);
                    break;
                }
            }
        }

        triggerCollider.isTrigger = false;
    }

    private void ToggleComponents(List<MonoBehaviour> components, bool newEnabledState)
    {
        foreach (MonoBehaviour component in components)
        {
            component.enabled = newEnabledState;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckIfValidReceiver(collision.gameObject) == false)
        {
            return;
        }

        receiversInRange.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CheckIfValidReceiver(collision.gameObject) == false)
        {
            return;
        }

        receiversInRange.Remove(collision.gameObject);
    }

    private bool CheckIfValidReceiver(GameObject gameObject)
    {
        foreach (GameObject receiver in validReceivers)
        {
            if (GameObject.ReferenceEquals(receiver, gameObject))
            {
                return true;
            }
        }
        return false;
    }

    private void ResolveReceiverInteractions()
    {
        if (receiversInRange.Count == 0)
        {
            AudioController.Instance.PlaySoundEffect(dropped_Sound, false);
            return;
        }

        if (resolveMultipleReceiversByDistance == true
            && receiversInRange.Count > 1)
        {
            SortReceiversByDistance();
        }

        foreach (GameObject receiver in receiversInRange)
        {
            ItemReceipt itemReceived = receiver.GetComponent<DragAndDropReceiver_Reference>().GetDragAndDropReceiver().ReceiveItem(gameObject);

            if (itemReceived != ItemReceipt.ItemRejected)
            {
                ToggleComponents(onReceivedToggleOffComponents, false);
                if (itemReceived == ItemReceipt.ItemReceived_ItemValid)
                {
                    AudioController.Instance.PlaySoundEffect(received_Sound, false);
                }
                else if (itemReceived == ItemReceipt.ItemReceived_ItemInvalid)
                {
                    AudioController.Instance.PlaySoundEffect(dropped_Sound, false);
                }

                return;
            }
        }

        if (negativeFeedback != null)
        {
            negativeFeedback.ExecuteNegativeFeedback();
        }
        else
        {
            AudioController.Instance.PlaySoundEffect(dropped_Sound, false);
        }
    }

    private void SortReceiversByDistance()
    {
        receiversInRange.Sort(delegate (GameObject a, GameObject b)
        {
            return Vector2.Distance(gameObject.transform.position, a.transform.position)
                .CompareTo(Vector2.Distance(gameObject.transform.position, b.transform.position));
        });
    }

    private void UpdateReceiversInRange()
    {
        receiversInRange.Clear();
        List<Collider2D> collider2Ds = new List<Collider2D>();

        triggerCollider.OverlapCollider(contactFilter, collider2Ds);

        foreach (Collider2D collider in collider2Ds)
        {
            if (CheckIfValidReceiver(collider.gameObject) == true)
            {
                receiversInRange.Add(collider.gameObject);
            }
        }
    }
}
