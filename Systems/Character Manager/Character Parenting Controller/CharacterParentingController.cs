using UnityEngine;

public class CharacterParentingController : MonoBehaviour
{
    [SerializeField] private Transform characterRoot;
    private Transform baseParent;
    private Transform currentParent;

    public delegate void CharacterParentChangeRequestedEvent(Transform newParent);
    public static event CharacterParentChangeRequestedEvent OnCharacterParentChangeRequested;

    public delegate void CharacterParentResetRequestedEvent();
    public static event CharacterParentResetRequestedEvent OnCharacterParentResetRequested;

    public Transform CurrentParent => currentParent;
    
    public static void ChangeParent(Transform newParent)
    {
        OnCharacterParentChangeRequested?.Invoke(newParent);
    }

    public static void ResetParent()
    {
        OnCharacterParentResetRequested?.Invoke();
    }

    private void SetParent(Transform newParent)
    {
        if (newParent != null)
        {
            currentParent = newParent;
            characterRoot.SetParent(newParent);
        }
    }

    private void ResetParentToBase()
    {
        currentParent = baseParent;
        characterRoot.SetParent(baseParent);
    }

    private void Awake()
    {
        baseParent = characterRoot.parent;
    }

    private void OnEnable()
    {
        OnCharacterParentChangeRequested += SetParent;
        OnCharacterParentResetRequested += ResetParentToBase;
    }

    private void OnDisable()
    {
        OnCharacterParentChangeRequested -= SetParent;
        OnCharacterParentResetRequested -= ResetParentToBase;
    }
}
