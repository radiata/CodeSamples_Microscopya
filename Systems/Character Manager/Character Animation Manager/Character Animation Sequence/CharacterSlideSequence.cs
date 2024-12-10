[System.Serializable]
public class CharacterSlideSequence : Base_CharacterAnimationSequence
{
    public static CharacterAnimationSequenceStart OnCharacterSlideSequenceStart;
    public static CharacterAnimationSequenceEnd OnCharacterSlideSequenceEnd;

    public CharacterSlideSequence(float animationDuration, CharacterAnimationState endSequenceState) : base(animationDuration, endSequenceState)
    {
    }

    public override void InitializeSequence()
    {
        OnCharacterSlideSequenceStart?.Invoke(animationDuration);
    }

    public override void EndSequence()
    {
        OnCharacterSlideSequenceEnd?.Invoke(endSequenceState);
    }
}
