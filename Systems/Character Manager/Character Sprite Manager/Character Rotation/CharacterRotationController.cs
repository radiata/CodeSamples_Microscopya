using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterRotationController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject model;

    private float rotationSpeed;

    private FacingDirection cachedFacingDirection;

    private NavigationObject cachedNavigationObject;
    private Quaternion targetRotation;

    public Quaternion TargetRotation => targetRotation;
    public FacingDirection FacingDirection => cachedFacingDirection;

    private static readonly Quaternion targetRotationModifier = Quaternion.Euler(0f, 0f, 90f);

    public void SetRotationSpeed(float newRotationSpeed)
    {
        rotationSpeed = newRotationSpeed;
    }

    public void SnapRotation()
    {
        UpdateRotation(Mathf.Infinity);
    }

    private void UpdateRotation(float maxRotation)
    {
        if (cachedNavigationObject != null)
        {
            targetRotation = cachedNavigationObject.GetRotationBasedOnLocation(navMeshAgent.transform.position, cachedFacingDirection);
            targetRotation *= targetRotationModifier;
        }

        model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, targetRotation, maxRotation);
    }

    private void UpdateCachedNavigationObject(NavigationObject navigationObject)
    {
        cachedNavigationObject = navigationObject;
    }

    private void UpdateFacingDirection(FacingDirection facingDirection)
    {
        Quaternion facingRotation = facingDirection == FacingDirection.left ? Quaternion.Euler(new Vector3(0, 180, 0)) : Quaternion.Euler(new Vector3(0, -180, 0));
        model.transform.rotation = model.transform.rotation * facingRotation;
        cachedFacingDirection = facingDirection;
    }

    private void InitializeRotation(FacingDirection facingDirection)
    {
        model.transform.rotation = facingDirection == FacingDirection.left ? Quaternion.Euler(new Vector3(-90, -90, -90)) : Quaternion.Euler(new Vector3(90, 0, 0));
        cachedFacingDirection = facingDirection;

        CharacterFacingReporter.OnCharacterFacingChanged -= InitializeRotation;
        CharacterFacingReporter.OnCharacterFacingChanged += UpdateFacingDirection;
    }

    private void Update()
    {
        UpdateRotation(rotationSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        CharacterNavigationObjectReporter.OnNavigationObjectChanged += UpdateCachedNavigationObject;
        CharacterFacingReporter.OnCharacterFacingChanged += InitializeRotation;
    }

    private void OnDisable()
    {
        CharacterNavigationObjectReporter.OnNavigationObjectChanged -= UpdateCachedNavigationObject;
        CharacterFacingReporter.OnCharacterFacingChanged -= InitializeRotation;
        CharacterFacingReporter.OnCharacterFacingChanged -= UpdateFacingDirection;
    }

    private IEnumerator Start()
    {
        yield return null;
        SnapRotation();
    }
}
