public class CharacterCameraTiltController : CameraTiltController
{
    protected override void OnEnable()
    {
        base.OnEnable();
        CharacterFacingReporter.OnCharacterFacingChanged += SetFacingModifier;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CharacterFacingReporter.OnCharacterFacingChanged -= SetFacingModifier;
    }

    private void SetFacingModifier(FacingDirection facingDirection)
    {
        switch (facingDirection)
        {
            case FacingDirection.uninitialized:
                rotationModifier = 0f;
                break;
            case FacingDirection.left:
                rotationModifier = -1f;
                break;
            case FacingDirection.right:
                rotationModifier = 1f;
                break;
        }
    }
}
