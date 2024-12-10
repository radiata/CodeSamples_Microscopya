using UnityEngine;

public class DragToRotate : MonoBehaviour, I_DraggablePuzzlePiece
{
    [SerializeField] private RotationResponder_Reference rotationResponder_Reference;
    private I_RotationResponder rotationResponder;
    [SerializeField] private bool updateRotationResponder;

    [SerializeField] private bool snapFacingToMouse = false;
    [SerializeField] private bool rotationAxisZ = true;

    [SerializeField] private bool navigateToPuzzleOnInteract = true;
    [SerializeField] private PuzzleManager puzzleManager;

    [SerializeField] private bool scaleOnInteract = true;
    [SerializeField] private float scaleMultiplier = 1.5f;
    private Vector3 initialScale;

    [SerializeField] private SoundEffect dropped_Sound;
    [SerializeField] private SoundEffect onPickUp_Sound;
    [SerializeField] private SoundEffect whileInteracting_Sound;
    [SerializeField] private SoundEffect received_Sound;

    private float dragOffsetAngle;
    private Quaternion dragStartRotation;

    public void OnDragStart(Vector3 worldPosition)
    {
        if (navigateToPuzzleOnInteract)
        {
            puzzleManager.Navigate();
        }

        initialScale = transform.localScale;

        if (scaleOnInteract)
        {
            transform.localScale = initialScale * scaleMultiplier;
        }

        if (updateRotationResponder)
        {
            if (rotationAxisZ == false)
            {
                Debug.Log("Only Z axis rotation is currently supported");
                throw new System.NotImplementedException();
            }

            rotationResponder.StartRotation(transform.rotation.eulerAngles.z);
        }

        dragOffsetAngle = FindMouseAngle(worldPosition);
        dragStartRotation = transform.rotation;

        AudioController.Instance.PlaySoundEffect(onPickUp_Sound, false);
        AudioController.Instance.PlaySoundEffect(whileInteracting_Sound, true);
    }

    public void WhileDragging(Vector3 worldPosition, Vector3 cameraForward)
    {
        var angle = FindMouseAngle(worldPosition) - dragOffsetAngle;
        HandleRotation(angle);

        if (updateRotationResponder)
        {
            if (rotationAxisZ == false)
            {
                Debug.Log("Only Z axis rotation is currently supported");
                throw new System.NotImplementedException();
            }

            rotationResponder.UpdateRotation(transform.rotation.eulerAngles.z);
        }
    }

    public void OnDragEnd(Vector3 worldPosition)
    {
        if (scaleOnInteract)
        {
            transform.localScale = initialScale;
        }

        if (updateRotationResponder)
        {
            bool? resultState = rotationResponder.EndRotation(transform.rotation.eulerAngles.z);
            ResolveResponderInteractions(resultState);
        }

        AudioController.Instance.StopSoundEffect();
    }

    private void ResolveResponderInteractions(bool? resultState)
    {
        if (resultState != null)
        {
            if (resultState.Value)
            {
                AudioController.Instance.PlaySoundEffect(received_Sound, false);
            }
            else
            {
                AudioController.Instance.PlaySoundEffect(dropped_Sound, false);
            }
        }
    }

    private void Start()
    {
        rotationResponder = rotationResponder_Reference.GetRotationResponder();
    }

    private float FindMouseAngle(Vector3 worldPosition)
    {
        Vector3 pivotPosition = transform.position;
        Vector3 mousePosition = worldPosition;

        var rPosition = new Vector2(mousePosition.x - pivotPosition.x, mousePosition.y - pivotPosition.y);

        var angle = Vector2.SignedAngle(Vector2.up, rPosition);
        return angle;
    }

    private void HandleRotation(float rotationAngle)
    {
        if (rotationAxisZ == false)
        {
            Debug.Log("Only Z axis rotation is currently supported");
            throw new System.NotImplementedException();
        }

        if (snapFacingToMouse == false)
        {
            transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward) * dragStartRotation;
        }
        else
        {
            rotationAngle += dragOffsetAngle;
            transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        }
    }
}
