using UnityEngine;

public class Disc : MonoBehaviour, I_RotationResponder
{
    [SerializeField] private float solutionAngle;
    [SerializeField] private float angleTolerance;
    private float currentAngle;

    [SerializeField] private float initialAngle;

    [SerializeField] private Collider2D discCollider;
    [SerializeField] private DragToRotate dragToRotate;

    public delegate void DiscSolvedEvent();
    public event DiscSolvedEvent OnDiscSolved;

    private bool solved = false;
    public bool isSolved => solved;

    public void StartRotation(float initialRotation)
    { }

    public void UpdateRotation(float currentRotation)
    { }

    public bool? EndRotation(float finalRotation)
    {
        currentAngle = finalRotation;
        HandleSolution();
        return solved;
    }

    private void HandleSolution()
    {
        if (Mathf.Abs(currentAngle - solutionAngle) > angleTolerance)
        {
            return;
        }

        transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, solutionAngle);

        solved = true;
        RemoveInteractivity();
        OnDiscSolved?.Invoke();
    }

    public void InitializeDisc(bool initializeAsSolved = false)
    {
        if (initializeAsSolved == false)
        {
            transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, initialAngle);
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, solutionAngle);
        }
        
        currentAngle = transform.eulerAngles.z;

        HandleSolution();
    }

    public void EnableInteractivity()
    {
        if(dragToRotate == null)
        {
            return;
        }
        discCollider.gameObject.layer = LayerReferences.InteractablePuzzleObjectsLayer;
    }

    private void RemoveInteractivity()
    {
        discCollider.gameObject.layer = LayerReferences.NonInteractableLayer;
        
        if (dragToRotate != null)
        {
            Destroy(dragToRotate);
        }
    }
}
