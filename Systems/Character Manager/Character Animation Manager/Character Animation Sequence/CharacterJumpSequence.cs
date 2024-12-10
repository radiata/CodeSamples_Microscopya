[System.Serializable]
public class CharacterJumpSequence : Base_CharacterAnimationSequence
{
    public static CharacterAnimationSequenceStart OnCharacterJumpSequenceStart;
    public static CharacterAnimationSequenceEnd OnCharacterJumpSequenceEnd;

    public CharacterJumpSequence(float animationDuration, CharacterAnimationState endSequenceState) : base(animationDuration, endSequenceState)
    {
    }

    public override void InitializeSequence()
    {
        OnCharacterJumpSequenceStart?.Invoke(animationDuration);
    }

    public override void EndSequence()
    {
        OnCharacterJumpSequenceEnd?.Invoke(endSequenceState);
    }
}
