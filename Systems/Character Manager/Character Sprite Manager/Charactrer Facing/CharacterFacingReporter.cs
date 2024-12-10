using UnityEngine.AI;
using UnityEngine;

public class CharacterFacingReporter : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject characterModel;
    [SerializeField] private FacingDirection defaultFacingDirection = FacingDirection.left;

    public delegate void CharacterFacingChangedEvent(FacingDirection facingDirection);
    public static event CharacterFacingChangedEvent OnCharacterFacingChanged;

    private FacingDirection facingDirection = FacingDirection.uninitialized;
    public FacingDirection FacingDirection => facingDirection;

    private NavigationObject cachedNavigationObject;
    private bool forceFacingDirection = false;

    private Vector3 localPosition;

    private bool lockFacingDirection = false;

    public void SetDefaultFacingDirection(FacingDirection newDefaultFacingDirection)
    {
        defaultFacingDirection = newDefaultFacingDirection;
    }

    public void ChangeFacingDirection(FacingDirection newFacingDirection)
    {
        if (facingDirection == newFacingDirection)
        {
            return;
        }

        UpdateDirection(newFacingDirection);
    }

    public void LockFacingDirection(FacingDirection facingDirection)
    {
        UpdateDirection(facingDirection);
        lockFacingDirection = true;
    }

    public void UnlockFacingDirection()
    {
        lockFacingDirection = false;
    }

    private void Update()
    {
        if (navMeshAgent.path.corners.Length > 1)
        {
            UpdateDirection(navMeshAgent.path.corners[1]);
        }
    }

    private void Start()
    {
        if (facingDirection == FacingDirection.uninitialized)
        {
            if (defaultFacingDirection == FacingDirection.uninitialized)
            {
                defaultFacingDirection = FacingDirection.left;
                Debug.LogWarning("Default Facing was set to Uninitialized during Start(), overriding with Left!");
                //If left as uninitialized, the entire rotation system will not function correctly!
            }
            facingDirection = defaultFacingDirection;
        }
        OnCharacterFacingChanged?.Invoke(facingDirection);
    }

    private void UpdateDirection(FacingDirection forcedDirection)
    {
        if (facingDirection != forcedDirection)
        {
            facingDirection = forcedDirection;
            OnCharacterFacingChanged?.Invoke(facingDirection);
        }
    }

    private void UpdateDirection(Vector3 nextLocationWorldPosition)
    {
        if (lockFacingDirection == true)
        {
            return;
        }

        if (forceFacingDirection == true)
        {
            if (facingDirection != cachedNavigationObject.FacingDirection)
            {
                facingDirection = cachedNavigationObject.FacingDirection;
                OnCharacterFacingChanged?.Invoke(facingDirection);
            }

            return;
        }

        localPosition = characterModel.transform.InverseTransformPoint(nextLocationWorldPosition).normalized;

        if (localPosition.x >= 0)
        {
            return;
        }

        if (facingDirection == FacingDirection.right)
        {
            facingDirection = FacingDirection.left;
        }
        else if (facingDirection == FacingDirection.left)
        {
            facingDirection = FacingDirection.right;
        }

        OnCharacterFacingChanged?.Invoke(facingDirection);
    }

    private void OnNavigationObjectChanged(NavigationObject navigationObject)
    {
        cachedNavigationObject = navigationObject;

        if (navigationObject == null || navigationObject.ForceFacingDirection == false)
        {
            forceFacingDirection = false;
            return;
        }

        if (navigationObject.ForceFacingDirection == true)
        {
            forceFacingDirection = true;
            return;
        }
    }

    private void OnEnable()
    {
        CharacterNavigationObjectReporter.OnNavigationObjectChanged += OnNavigationObjectChanged;
        CharacterFacingLock.OnCharacterFacingLocked += LockFacingDirection;
        CharacterFacingLock.OnCharacterFacingUnlocked += UnlockFacingDirection;
    }

    private void OnDisable()
    {
        CharacterNavigationObjectReporter.OnNavigationObjectChanged -= OnNavigationObjectChanged;
        CharacterFacingLock.OnCharacterFacingLocked -= LockFacingDirection;
        CharacterFacingLock.OnCharacterFacingUnlocked -= UnlockFacingDirection;
    }
}
