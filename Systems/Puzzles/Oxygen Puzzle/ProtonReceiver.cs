using UnityEngine;

public class ProtonReceiver : MonoBehaviour, I_DragAndDropReceiver
{
    [SerializeField] private OxyProtonSpawner oxyProtonSpawner;

    private bool active = false;

    public delegate void ProtonReceivedEvent();
    public event ProtonReceivedEvent OnProtonReceived;

    public ItemReceipt ReceiveItem(GameObject gameObject)
    {
        if(active == false)
        {
            return ItemReceipt.ItemRejected;
        }

        OnProtonReceived?.Invoke();
        oxyProtonSpawner.RecyclePoolUnit(gameObject, true);
        oxyProtonSpawner.ReplenishPool();

        return ItemReceipt.ItemReceived_ItemValid;
    }

    public void SetActive()
    {
        active = true;
    }

    public void SetInactive()
    {
        active = false;
    }
}
