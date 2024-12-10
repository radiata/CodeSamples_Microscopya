using UnityEngine;

public interface I_SwitchPuzzlePiece
{
    public abstract void SwitchToggle();
    public abstract void OnDragStart(Vector3 worldPosition);
    public abstract void WhileDragging(Vector3 worldPosition, Vector3 cameraForward);
    public abstract void OnDragEnd(Vector3 worldPosition);
}
