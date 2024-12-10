[System.Serializable]
public class CharacterMotorPuzzleJumpSequence : Base_CharacterAnimationSequence
{
    public static CharacterAnimationSequenceStart OnCharacterMotorPuzzleJumpSequenceStart;
    public static CharacterAnimationSequenceEnd OnCharacterMotorPuzzleJumpSequenceEnd;

    public CharacterMotorPuzzleJumpSequence(float animationDuration, CharacterAnimationState endSequenceState) : base(animationDuration, endSequenceState)
    {
    }

    public override void InitializeSequence()
    {
        OnCharacterMotorPuzzleJumpSequenceStart?.Invoke(animationDuration);
    }

    public override void EndSequence()
    {
        OnCharacterMotorPuzzleJumpSequenceEnd?.Invoke(endSequenceState);
    }
}
