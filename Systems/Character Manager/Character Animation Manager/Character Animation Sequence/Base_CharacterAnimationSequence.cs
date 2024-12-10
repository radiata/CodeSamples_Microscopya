using UnityEngine;

[System.Serializable]
public abstract class Base_CharacterAnimationSequence
{
    [SerializeField] protected float animationDuration;
    [SerializeField] protected CharacterAnimationState endSequenceState;

    public delegate void CharacterAnimationSequenceStart(float duration);
    public delegate void CharacterAnimationSequenceEnd(CharacterAnimationState endSequenceState);

    protected Base_CharacterAnimationSequence(float animationDuration, CharacterAnimationState endSequenceState)
    {
        this.animationDuration = animationDuration;
        this.endSequenceState = endSequenceState;
    }

    public abstract void InitializeSequence();
    public abstract void EndSequence();
}
