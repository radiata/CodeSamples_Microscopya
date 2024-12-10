using UnityEngine;

public class ProteinPuzzle_Rotator : MonoBehaviour, I_DraggablePuzzlePiece
{
    [SerializeField] private GameObject rotationObject;
    [SerializeField] private GameObject puzzleObject;
    [SerializeField] private ProteinRotationPuzzle proteinRotationPuzzle;

    [SerializeField] private float degreesPerDistance = 5f;

    [SerializeField] private float easeBackTime = 0f;

    private Quaternion initialRotation = Quaternion.identity;
    private Vector3 mouseStart = Vector3.zero;

    private Transform puzzleObjectParent;

    public void AssignVariables(GameObject rotationObject, ProteinRotationPuzzle proteinRotationPuzzle, GameObject puzzleObject)
    {
        this.rotationObject = rotationObject;
        this.proteinRotationPuzzle = proteinRotationPuzzle;
        this.puzzleObject = puzzleObject;
    }

    void I_DraggablePuzzlePiece.OnDragStart(Vector3 worldPosition)
    {
        puzzleObjectParent = puzzleObject.transform.parent;
        rotationObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        puzzleObject.transform.SetParent(rotationObject.transform);

        initialRotation = rotationObject.transform.rotation;
        mouseStart = worldPosition;
    }

    void I_DraggablePuzzlePiece.WhileDragging(Vector3 worldPosition, Vector3 cameraForward)
    {
        var yDif = worldPosition.x - mouseStart.x;
        var xDif = worldPosition.y - mouseStart.y;
        var zDif = worldPosition.z - mouseStart.z;

        Vector3 rotationAmount = new Vector3(xDif, -yDif, zDif) * degreesPerDistance;
        rotationObject.transform.rotation = initialRotation * Quaternion.Euler(rotationAmount);
    }

    void I_DraggablePuzzlePiece.OnDragEnd(Vector3 worldPosition)
    {
        puzzleObject.transform.SetParent(puzzleObjectParent);
        rotationObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        proteinRotationPuzzle.ResetViewingAngle(easeBackTime);
    }
}
