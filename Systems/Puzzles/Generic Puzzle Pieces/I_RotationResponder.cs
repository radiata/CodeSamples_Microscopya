public interface I_RotationResponder
{
    public abstract void StartRotation(float initialRotation);
    public abstract void UpdateRotation(float currentRotation);
    public abstract bool? EndRotation(float finalRotation);
}
