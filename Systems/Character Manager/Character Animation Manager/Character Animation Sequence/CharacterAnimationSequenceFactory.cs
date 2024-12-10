public static class CharacterAnimationSequenceFactory
{
    public static Base_CharacterAnimationSequence CreateSequence(CharacterAnimationSequences sequenceType, float animationDuration, CharacterAnimationState endSequenceState)
    {
        switch (sequenceType)
        {
            case CharacterAnimationSequences.None:
                return null;
            case CharacterAnimationSequences.JumpSequence:
                return CreateJumpSequence(animationDuration, endSequenceState);
            case CharacterAnimationSequences.SlideSequence:
                return CreateSlideSequence(animationDuration, endSequenceState);
            case CharacterAnimationSequences.MotorPuzzleJumpSequence:
                return CreateJumpSequence_MotorPuzzle(animationDuration, endSequenceState);
            case CharacterAnimationSequences.RunSequence:
                return CreateRunSequence(animationDuration, endSequenceState);
        }

        throw new System.Exception($"Sequence Type Unrecognized, add a case for this sequence to {nameof(CharacterAnimationSequenceFactory)}");
    }

    private static CharacterJumpSequence CreateJumpSequence(float animationDuration, CharacterAnimationState endSequenceState)
    {
        return new CharacterJumpSequence(animationDuration, endSequenceState);
    }

    private static CharacterSlideSequence CreateSlideSequence(float animationDuration, CharacterAnimationState endSequenceState)
    {
        return new CharacterSlideSequence(animationDuration, endSequenceState);
    }

    private static CharacterMotorPuzzleJumpSequence CreateJumpSequence_MotorPuzzle(float animationDuration, CharacterAnimationState endSequenceState)
    {
        return new CharacterMotorPuzzleJumpSequence(animationDuration, endSequenceState);
    }

    private static CharacterRunSequence CreateRunSequence(float animationDuration, CharacterAnimationState endSequenceState)
    {
        return new CharacterRunSequence(animationDuration, endSequenceState);
    }
}
