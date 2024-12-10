public static class CharacterFacingLock
{
    public delegate void CharacterFacingLockEvent(FacingDirection facingDirection);
    public static event CharacterFacingLockEvent OnCharacterFacingLocked;

    public delegate void CharacterFacingUnlockEvent();
    public static event CharacterFacingUnlockEvent OnCharacterFacingUnlocked;

    public static void CharacterFacingLockRequest(FacingDirection facingDirection)
    {
        OnCharacterFacingLocked?.Invoke(facingDirection);
    }

    public static void CharacterFacingUnlockRequest()
    {
        OnCharacterFacingUnlocked?.Invoke();
    }
}
