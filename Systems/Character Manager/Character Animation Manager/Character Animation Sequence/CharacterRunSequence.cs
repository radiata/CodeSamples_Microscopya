[System.Serializable]
public class CharacterRunSequence : Base_CharacterAnimationSequence
{
    public static CharacterAnimationSequenceStart OnCharacterRunSequenceStart;
    public static CharacterAnimationSequenceEnd OnCharacterRunSequenceEnd;

    public CharacterRunSequence(float animationDuration, CharacterAnimationState endSequenceState) : base(animationDuration, endSequenceState)
    { }

    public override void InitializeSequence()
    {
        OnCharacterRunSequenceStart?.Invoke(animationDuration);
    }

    public override void EndSequence()
    {
        OnCharacterRunSequenceEnd?.Invoke(endSequenceState);
    }
}
